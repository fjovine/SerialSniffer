using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace SerialSniffer
{
    /// <summary>
    /// Logica di interazione per Gui.xaml
    /// </summary>
    public partial class Gui : Window
    {
        volatile bool startRequest = false;
        public Gui()
        {
            InitializeComponent();

            Thread runner = new Thread(() =>
            {
                while (true)
                {
                    if (startRequest)
                    {
                        DateTime start = DateTime.MinValue;
                        bool isFirst = true;

                        Sniffer sniffer = new Sniffer(
                            GlobalParameters.VirtualPort,
                            GlobalParameters.RealPort,
                            GlobalParameters.BaudRate,
                            GlobalParameters.TransmissionParity,
                            GlobalParameters.TransmissionStopBits,
                            GlobalParameters.TransmissionDataBits);

                        sniffer.IsCollapsingSameOrigin = GlobalParameters.IsShowCollapsed;
                        //TextWriter outputFile = new StreamWriter(GlobalParameters.OutputFileName);

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
                                par.Background = (ee.Origin == Origin.FromReal) ? Brushes.AliceBlue: Brushes.OldLace;
                                par.Inlines.Add(sniffed);
                                this.SnifferOuput.Document.Blocks.Add(par);
                                this.SnifferOuput.ScrollToEnd();
                            }));
                        };
                        sniffer.OpenAndSniff();
                        startRequest = false;
                    }
                }
            });
            runner.Start();
        }

        private void StartStop_Click(object sender, RoutedEventArgs e)
        {
            this.StartStop.IsEnabled = false;
            this.startRequest = true;
        }
    }
}
