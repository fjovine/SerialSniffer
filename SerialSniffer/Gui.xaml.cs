//-----------------------------------------------------------------------
// <copyright file="Gui.xaml.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------

namespace SerialSniffer
{
    using System;
    using System.Collections.Generic;
    using System.IO.Ports;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    /// <summary>
    /// Interaction with <c>Gui.xaml</c> that is the main GUI for the serial sniffer.
    /// </summary>
    public partial class Gui : Window
    {
        /// <summary>
        /// Color to be used as text box background in case of error
        /// </summary>
        private static readonly Brush ErrorColor = Brushes.Pink;

        /// <summary>
        /// Color to be used as text box background when no error has been found
        /// </summary>
        private static readonly Brush OkColor = Brushes.White;

        /// <summary>
        /// Flag that signals the request to start sniffing the serial line.
        /// </summary>
        private volatile bool startRequest = false;

        /// <summary>
        /// Flag that signals the request to stop sniffing the serial line
        /// </summary>
        private volatile bool stopRequest = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Gui" /> class.
        /// </summary>
        public Gui()
        {
            this.InitializeComponent();
            this.StartStop.IsEnabled = !this.CheckInputFieldsError();

            Thread runner = new Thread(() =>
            {
                while (true)
                {
                    if (startRequest)
                    {
                        Sniffer sniffer = new Sniffer(
                            GlobalParameters.VirtualPort,
                            GlobalParameters.RealPort,
                            GlobalParameters.TransmissionBaudRate,
                            GlobalParameters.TransmissionParity,
                            GlobalParameters.TransmissionStopBits,
                            GlobalParameters.TransmissionDataBits);

                        DateTime start = DateTime.MinValue;
                        bool isFirst = true;

                        sniffer.IsCollapsingSameOrigin = GlobalParameters.IsShowCollapsed;

                        sniffer.Available += (s, ee) =>
                        {
                            if (isFirst)
                            {
                                start = ee.When;
                                isFirst = false;
                            }

                            string sniffed = Program.DecodeArrivedPacket(ee, start);
                            this.SnifferOuput.Dispatcher.BeginInvoke((Action)(() =>
                            {
                                // this.SnifferOuput.Document.
                                Paragraph par = new Paragraph();
                                par.Margin = new Thickness(0);
                                if (ee.Origin == Origin.FromReal)
                                {
                                    par.Background = Brushes.AliceBlue;
                                    par.Foreground = Brushes.Blue;
                                }
                                else
                                {
                                    par.Background = Brushes.OldLace;
                                    par.Foreground = Brushes.Red;
                                }

                                par.Background = (ee.Origin == Origin.FromReal) ? Brushes.AliceBlue : Brushes.OldLace;
                                par.Inlines.Add(sniffed);
                                this.SnifferOuput.Document.Blocks.Add(par);
                                this.SnifferOuput.ScrollToEnd();
                            }));
                        };
                        try
                        {
                            sniffer.OpenAndSniff();
                        }
                        catch (System.IO.IOException)
                        {
                            MessageBox.Show("Connection error: probably one ports has a wrong name");
                        }

                        startRequest = false;
                    }

                    if (stopRequest)
                    {
                        stopRequest = false;
                        break;
                    }
                }
            });
            runner.Start();
        }

        /// <summary>
        /// Event handler called when the start stop button on the GUI is pressed.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void StartStop_Click(object sender, RoutedEventArgs e)
        {
            this.StartStop.IsEnabled = false;
            this.startRequest = true;

            GlobalParameters.VirtualPort = this.VirtualPort.Text;
            GlobalParameters.RealPort = this.RealPort.Text;
            GlobalParameters.BytesPerLine = int.Parse(this.BytesPerLine.Text);
            GlobalParameters.TransmissionBaudRate = int.Parse(this.Baud.Text);
            GlobalParameters.TransmissionStopBits = GlobalParameters.Value2StopBits[this.StopBits.Text];
            GlobalParameters.TransmissionParity = this.Parity.Text.ToEnum<Parity>();
            GlobalParameters.TransmissionDataBits = int.Parse(this.DataBits.Text);

            GlobalParameters.IsOnlyAscii = this.OnlyAscii.IsChecked == true;
            GlobalParameters.IsOnlyHex = this.OnlyHex.IsChecked == true;
            GlobalParameters.IsShowTime = this.Time.IsChecked == true;
            GlobalParameters.IsShowCollapsed = this.ShowCollapsed.IsChecked == true;

            GlobalParameters.SaveToRegistry();
        }

        /// <summary>
        /// Event handler for the closure of the main window
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.stopRequest = true;
        }

        /// <summary>
        /// Event handler that checks if the content of the textbox is a valid filename
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void VirtualPorts_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            this.StartStop.IsEnabled = !this.CheckInputFieldsError();
        }

        /// <summary>
        /// Checks if all the input fields are in an acceptable state.
        /// </summary>
        /// <returns>True if one of the fields is wrong.</returns>
        private bool CheckInputFieldsError()
        {
            bool result = this.CheckFilenameError(this.VirtualPort);
            result |= this.CheckFilenameError(this.RealPort);
            return result;
        }

        /// <summary>
        /// Verifies that the passed text box contains a port filename inside the list of serial ports retrieved from the operating system.
        /// If there is an error, the text box background color is changed accordingly
        /// </summary>
        /// <param name="textBox">Text box control containing the port name</param>
        /// <returns>True if the name is not a valid serial port name</returns>
        private bool CheckFilenameError(TextBox textBox)
        {
            string [] ports = SerialPort.GetPortNames();

            bool result;
            string filename = textBox.Text;
            if (Array.IndexOf(ports, filename) == -1)
            {
                textBox.Background = ErrorColor;
                result = true;
            }
            else
            {
                textBox.Background = OkColor;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Event handler triggered by the loaded event. When the main window gets loaded, the parameters are loaded from registry
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.VirtualPort.Text = WindowsRegistry.Get(WindowsRegistryEntry.VIRTUALPORT, string.Empty);
            this.RealPort.Text = WindowsRegistry.Get(WindowsRegistryEntry.REALPORT, string.Empty);

            WindowsRegistry.Get(WindowsRegistryEntry.BYTESPERLINE, this.BytesPerLine, "16");
            WindowsRegistry.Get(WindowsRegistryEntry.BAUDRATE, this.Baud, "9600");
            WindowsRegistry.Get(WindowsRegistryEntry.PARITY, this.Parity, "None");
            WindowsRegistry.Get(
                WindowsRegistryEntry.STOPBIT, 
                this.StopBits, 
                "One",
                new Dictionary<string, string>()
                {
                    { "None", "0" },
                    { "One", "1" },
                    { "OnePointFive", "1.5" },
                    { "Two", "2" }
                });
            WindowsRegistry.Get(WindowsRegistryEntry.DATA, this.DataBits, "8");

            bool boolTemp;
            WindowsRegistry.Get(WindowsRegistryEntry.ISONLYHEX, out boolTemp, false);
            this.OnlyHex.IsChecked = boolTemp;
            WindowsRegistry.Get(WindowsRegistryEntry.ISONLYASCII, out boolTemp, false);
            this.OnlyAscii.IsChecked = boolTemp;
            WindowsRegistry.Get(WindowsRegistryEntry.ISCOLLAPSED, out boolTemp, false);
            this.ShowCollapsed.IsChecked = boolTemp;
            WindowsRegistry.Get(WindowsRegistryEntry.ISTIME, out boolTemp, false);
            this.Time.IsChecked = boolTemp;
        }
    }
}