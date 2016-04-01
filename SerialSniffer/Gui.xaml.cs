//-----------------------------------------------------------------------
// <copyright file="Gui.xaml.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------

namespace SerialSniffer
{
    using System;
    using System.Threading;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Media;

    /// <summary>
    /// Interaction with <c>Gui.xaml</c> that is the main GUI for the serial sniffer.
    /// </summary>
    public partial class Gui : Window
    {
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

            Thread runner = new Thread(() =>
            {
                while (true)
                {
                    if (startRequest)
                    {
                        Sniffer sniffer = new Sniffer(
                            GlobalParameters.VirtualPort,
                            GlobalParameters.RealPort,
                            GlobalParameters.BaudRate,
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
    }
}
