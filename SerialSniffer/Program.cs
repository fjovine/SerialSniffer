//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------
namespace SerialSniffer
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows;
    using Bsc;

    /// <summary>
    /// Main class of the SerialSniffer.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Win32 code to hide a window
        /// </summary>
        private const int SWHIDE = 0;

        /// <summary>
        /// Win32 code to show a window
        /// </summary>
        private const int SWSHOW = 5;

        /// <summary>
        /// Computes a string that shows a decoded version of the sniffed packet.
        /// </summary>
        /// <param name="e">Argument object containing the packed to be decoded.</param>
        /// <param name="start">Start time when the packet has been sniffed. This time is reliable when the <see cref="GlobalParameters.IsShowCollapsed"/> is false. When
        /// it is true, it should be considered that the packet could be composed of strings of characters loaded non contiguously.</param>
        /// <returns>>The decoded version of the sniffed packet.</returns>
        public static string DecodeArrivedPacket(SniffedPacketEventArgs e, DateTime start)
        {
            string preamble = string.Empty;

            if (GlobalParameters.IsShowTime)
            {
                preamble = string.Format(
                    "{0:yyyy-MM-dd HH mm ss.fff} {1} ",
                    e.When,
                    e.Origin == Origin.FromReal ? '<' : '>');
            }
            else
            {
                preamble = string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "{0,10:0.000} {1} ",
                    e.When.Subtract(start).TotalMilliseconds,
                    e.Origin == Origin.FromReal ? '<' : '>');
            }

            return e.Content.ToHex(preamble, GlobalParameters.OutputFormat, GlobalParameters.BytesPerLine);
        }

        /// <summary>
        /// Performs the sniffing.<para/>
        /// This function returns immediately as the sniffing process is handled through events. It returns
        /// the handler of the output file on which the sniffed data is stored.
        /// </summary>
        /// <returns>The TextWriter on which the sniffed data have been stored.</returns>
        public static TextWriter DoSniff()
        {
            DateTime start = DateTime.MinValue;
            bool isFirst = true;

            Sniffer sniffer = new Sniffer(
                GlobalParameters.VirtualPort,
                GlobalParameters.RealPort,
                GlobalParameters.TransmissionBaudRate,
                GlobalParameters.TransmissionParity,
                GlobalParameters.TransmissionStopBits,
                GlobalParameters.TransmissionDataBits);

            sniffer.IsCollapsingSameOrigin = GlobalParameters.IsShowCollapsed;

            TextWriter outputFile = null;
            if (GlobalParameters.OutputFileName.ToLower() == "stdout")
            {
                outputFile = Console.Out;
            }
            else
            {
                outputFile = new StreamWriter(GlobalParameters.OutputFileName);
            }

            sniffer.Available += (s, e) =>
            {
                if (isFirst)
                {
                    start = e.When;
                    isFirst = false;
                }

                outputFile.WriteLine(DecodeArrivedPacket(e, start));
            };
            sniffer.OpenAndSniff();
            return outputFile;
        }

        /// <summary>
        /// This method hides the console window
        /// </summary>
        public static void HideConsoleWindow()
        {
            var handle = GetConsoleWindow();

            ShowWindow(handle, SWHIDE);
        }

        /// <summary>
        /// Entry point of the utility.
        /// </summary>
        /// <param name="args">Command line.</param>
        [STAThread]
        public static void Main(string[] args)
        {
            TextWriter outputFile = null;
            try
            {
                GlobalParameters.ParseCommandLineArguments(args);
                if (GlobalParameters.IsGui)
                {
                    HideConsoleWindow();
                    Application application = new Application();
                    application.Run(new Gui());
                }
                else if (GlobalParameters.IsHelp)
                {
                    PrintUsage();
                }
                else
                {
                    outputFile = DoSniff();
                }
            }
            catch (CommandLineArgumentException e)
            {
                Console.WriteLine(e.Message);
                PrintUsage();
            }
            catch (IOException)
            {
                Console.WriteLine("Communication error: check if the port names are correct");
            }

            if (!GlobalParameters.IsGui)
            {
                Console.WriteLine("Press a key to stop sniffing");
                Console.ReadLine();
            }

            if (outputFile != null)
            {
                outputFile.Close();
            }
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        /// <summary>
        /// Prints out the synopsis of the command and exits.
        /// </summary>
        private static void PrintUsage()
        {
            Console.WriteLine();
            Console.WriteLine("USAGE:");
            Console.WriteLine("SerialSniffer.exe -virtual <string> -real <string> [-baud <int>] [-parity (none|odd|even)]");
            Console.WriteLine("                   [-stopbit (1|2|1.5)][-data (7|8)] [-onlyHex] [-onlyAscii] [-time]");
            Console.WriteLine();
            Console.WriteLine("OPTIONS:");
            Console.WriteLine("  -virtual : the name of the virtual (connected via con0con) port. Mandatory.");
            Console.WriteLine("  -real : the name of the real (connected to the device) port. Mandatory");
            Console.WriteLine("  -output : the name of the file where the sniffed data will be stored. Mandatory");
            Console.WriteLine();
            Console.WriteLine("  -baud: baud rate with the real device. Optional, default 9600");
            Console.WriteLine("  -parity: the parity of the communications. Optional, default to none");
            Console.WriteLine("  -stopbit: the number of communicaiton stopbits. Optional, default to 1");
            Console.WriteLine("  -data: Number of data bits. Optional, default is 8.");
            Console.WriteLine("  -onlyHex: Optional flag. If defined, then only the hex representation is generated.");
            Console.WriteLine("  -onlyAscii: Optional flag. If defined, then only the ASCII representation is generated.");
            Console.WriteLine("  -time: Optional flag. If defined, then the time when data arrives will be shown in the ");
            Console.WriteLine("         YYYY-MM-DD HH:mm:ss.fff format");
            Console.WriteLine("  -gui: Optional flag. If defined an interactive gui is shown");
            Console.WriteLine("  -bytesPerLine: Optional number of bytes shown per line. Default 16");
            Console.WriteLine("  -collapsed: if true, then the successive packets from the same origin a re shown together. Optional, default false");
            Console.WriteLine("  -help: this help description");
            Console.WriteLine("The default format contains both hex and ascii and the time is shown as milliseconds elapsed");
            Console.WriteLine("since the first packet has been sniffed.");
            Console.WriteLine();
            Console.WriteLine("EXAMPLE:");
            Console.WriteLine("1. > SerialSniffer -virtual COM1 -real COM2 -baud 9600 -output sniffed.txt");
            Console.ReadLine();
            Environment.Exit(1);
        }

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr windowHandle, int comandShow);
    }
}
