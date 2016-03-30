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
    using Bsc;

    /// <summary>
    /// Main class of the SerialSniffer.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point of the utility.
        /// </summary>
        /// <param name="args">Command line.</param>
        public static void Main(string[] args)
        {
            TextWriter outputFile = null;
            try
            {
                GlobalParameters.ParseCommandLineArguments(args);
                outputFile = DoSniff();
            }
            catch (CommandLineArgumentException e)
            {
                Console.WriteLine(e.Message);
                PrintUsage();
            }

            Console.ReadLine();
            if (outputFile != null)
            {
                outputFile.Close();
            }
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
                GlobalParameters.BaudRate,
                GlobalParameters.TransmissionParity,
                GlobalParameters.TransmissionStopBits,
                GlobalParameters.TransmissionDataBits);

            sniffer.IsCollapsingSameOrigin = GlobalParameters.IsShowCollapsed;
            ByteEnumerableExtensions.Format outputFormat = ByteEnumerableExtensions.Format.Plain;

            if (GlobalParameters.IsOnlyAscii)
            {
                outputFormat = ByteEnumerableExtensions.Format.OnlyAscii;
            }
            else if (GlobalParameters.IsOnlyHex)
            {
                outputFormat = ByteEnumerableExtensions.Format.OnlyHex;
            }

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
                        "{0,10:0.000} {1} ",
                        e.When.Subtract(start).TotalMilliseconds,
                        e.Origin == Origin.FromReal ? '<' : '>');
                }

                outputFile.WriteLine(e.Content.ToHex(preamble, outputFormat, GlobalParameters.BytesPerLine));
            };
            sniffer.OpenAndSniff();
            return outputFile;
        }

        /// <summary>
        /// Parses the command line arguments and shows help if applicable
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        private static void ParseArguments(string[] args)
        {
            // Handle the special case "-help" separately
            if (args.Length == 1 && args[0].Trim() == "-help")
            {
                PrintUsage();
            }
            else
            {
                CommandLineArgumentParser.ParseArguments(args);
            }
        }

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
            Console.WriteLine("  -bytesPerLine: Optiona number of bytes shown per line. Default 16");
            Console.WriteLine("  -help: this help description");
            Console.WriteLine("The default format contains both hex and ascii and the time is shown as milliseconds elapsed");
            Console.WriteLine("since the first packet has been sniffed.");
            Console.WriteLine();
            Console.WriteLine("EXAMPLE:");
            Console.WriteLine("1. > SerialSniffer -virtual COM1 -real COM2 -baud 9600 -output sniffed.txt");
            Environment.Exit(1);
        }
    }
}
