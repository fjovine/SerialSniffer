//-----------------------------------------------------------------------
// <copyright file="GlobalParameters.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------
namespace SerialSniffer
{
    using System.Collections.Generic;
    using System.IO.Ports;
    using Bsc;

    /// <summary>
    /// Contains the global parameters 
    /// </summary>
    public class GlobalParameters
    {
        /// <summary>
        /// Initializes static members of the<see cref="GlobalParameters" /> class.
        /// </summary>
        static GlobalParameters()
        {
            // Mandatory parameters of the application decoded from the command line or determined as default values.
            string[] requiredArguments =
            {
                "-virtual",
                "-real",
            };
            CommandLineArgumentParser.DefineRequiredParameters(requiredArguments);

            // Optional parameters
            string[] optionalArguments =
            {
                "-baud=9600",
                "-parity=none",
                "-stopbit=1",
                "-data=8",
                "-output=stdout",
                "-bytesPerLine=16"
            };
            CommandLineArgumentParser.DefineOptionalParameter(optionalArguments);

            // Supported switches
            string[] switches =
                {
                        "-onlyHex",
                        "-onlyAscii",
                        "-time",
                        "-collapsed",
                        "-gui"
                    };
            CommandLineArgumentParser.DefineSwitches(switches);
        }

        /// <summary>
        /// Gets the baud rate to be used in the communications
        /// </summary>
        public static int BaudRate
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of bytes to be contained in each line in the output file
        /// </summary>
        public static int BytesPerLine
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether a GUI should be shown
        /// </summary>
        public static bool IsGui
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the output format should contain only the ASCII data, with dots as placeholders for non printable bytes.
        /// </summary>
        public static bool IsOnlyAscii
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the output format should contain only hex decoded bytes
        /// </summary>
        public static bool IsOnlyHex
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the arriving packets should be collapsed, i.e. that all the subsequent data coming from the same source is collapsed
        /// in only one packet. If this option is chosen, the sniffed data will contain one packet coming from the device and the following coming from the software, i.e.
        /// it is impossible to have two successive packets coming from the same origin.
        /// </summary>
        public static bool IsShowCollapsed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the absolute time is to be shown instead of the elapsed time in milliseconds from the beginning of the sniffing session.
        /// </summary>
        public static bool IsShowTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the filename where sniffed data is stored.
        /// </summary>
        public static string OutputFileName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the real port connected to the external device.
        /// </summary>
        public static string RealPort
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of data bits to be used in the serial transmission.
        /// </summary>
        public static int TransmissionDataBits
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the parity to be used in the serial communication.
        /// </summary>
        public static Parity TransmissionParity
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the stop bits to be used in the serial communication.
        /// </summary>
        public static StopBits TransmissionStopBits
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the VirtualPort, i.e. the port managed by com0com and null-connected to another virtual port to which the software under test communicates.
        /// </summary>
        public static string VirtualPort
        {
            get;
            private set;
        }

        /// <summary>
        /// Parses the passed command line parameters and populates the static properties used as global parameters of this application.
        /// </summary>
        /// <param name="args">String array containing the command line parameters.</param>
        public static void ParseCommandLineArguments(string[] args)
        {
            CommandLineArgumentParser.ParseArguments(args);
            VirtualPort = CommandLineArgumentParser.GetParamValue("-virtual");
            RealPort = CommandLineArgumentParser.GetParamValue("-real");
            OutputFileName = CommandLineArgumentParser.GetParamValue("-output");
            BaudRate = int.Parse(CommandLineArgumentParser.GetParamValue("-baud"));
            BytesPerLine = int.Parse(CommandLineArgumentParser.GetParamValue("-bytesPerLine"));
            TransmissionParity = CommandLineArgumentParser.GetParamValue<Parity>(
                "-parity",
                new Dictionary<string, Parity>()
                {
                        { "even", Parity.Even },
                        { "odd", Parity.Odd },
                        { "none", Parity.None }
                });
            TransmissionStopBits = CommandLineArgumentParser.GetParamValue<StopBits>(
                "-stopbit",
                new Dictionary<string, StopBits>
                {
                        { "0", StopBits.None },
                        { "1", StopBits.One },
                        { "1.5", StopBits.OnePointFive },
                        { "2", StopBits.Two }
                });
            TransmissionDataBits = CommandLineArgumentParser.GetParamValue<int>(
                "-data",
                new Dictionary<string, int>()
                {
                        { "7", 7 },
                        { "8", 8 }
                });
            IsOnlyHex = CommandLineArgumentParser.IsSwitchOn("-onlyHex");
            IsOnlyAscii = CommandLineArgumentParser.IsSwitchOn("-onlyAscii");
            IsShowTime = CommandLineArgumentParser.IsSwitchOn("-time");
            IsGui = CommandLineArgumentParser.IsSwitchOn("-gui");
            IsShowCollapsed = CommandLineArgumentParser.IsSwitchOn("-collapsed");
        }
    }
}
