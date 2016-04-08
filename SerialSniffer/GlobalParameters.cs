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
        /// Maps the value used for the <c>stopbis</c> (GUI and line commands) to the enum items
        /// </summary>
        private static Dictionary<string, StopBits> value2StopBits = new Dictionary<string, StopBits>()
        {
                { "0", StopBits.None },
                { "1", StopBits.One },
                { "1.5", StopBits.OnePointFive },
                { "2", StopBits.Two }
        };

        /// <summary>
        /// Initializes static members of the<see cref="GlobalParameters" /> class.
        /// </summary>
        static GlobalParameters()
        {
            // Unique parameters that can be used alone
            string[] uniqueParameters =
            {
                "-gui",
                "-help"
            };
            CommandLineArgumentParser.DefineUniqueParameters(uniqueParameters);

            // Mandatory parameters of the application decoded from the command line.
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
                        "-collapsed"
                    };
            CommandLineArgumentParser.DefineSwitches(switches);
        }

        /// <summary>
        /// Gets a dictionary that Maps the value used for the <c>stopbis</c> (GUI and line commands) to the enum items
        /// </summary>
        public static Dictionary<string, StopBits> Value2StopBits
        {
            get
            {
                return GlobalParameters.value2StopBits;
            }
        }

        /// <summary>
        /// Gets or sets the number of bytes to be contained in each line in the output file
        /// </summary>
        public static int BytesPerLine
        {
            get;
            set;
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
        /// Gets a value indicating whether the help window should be shown.
        /// </summary>
        public static bool IsHelp
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the output format should contain only the ASCII data, with dots as placeholders for non printable bytes.
        /// </summary>
        public static bool IsOnlyAscii
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the output format should contain only hex decoded bytes
        /// </summary>
        public static bool IsOnlyHex
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the arriving packets should be collapsed, i.e. that all the subsequent data coming from the same source is collapsed
        /// in only one packet. If this option is chosen, the sniffed data will contain one packet coming from the device and the following coming from the software, i.e.
        /// it is impossible to have two successive packets coming from the same origin.
        /// </summary>
        public static bool IsShowCollapsed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the absolute time is to be shown instead of the elapsed time in milliseconds from the beginning of the sniffing session.
        /// </summary>
        public static bool IsShowTime
        {
            get;
            set;
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
        /// Gets the output format decoding two GlobalParameters: <see cref="GlobalParameters.IsOnlyAscii"/> and <see cref="GlobalParameters.IsOnlyHex"/>
        /// If none are set, then <see cref="ByteEnumerableExtensions.Format.Plain"/> is returned, otherwise <c>IsOnlyAscii</c> has the priority over the other predicate, i.e.
        /// if <c>IsOnlyAscii</c> is true, <see cref="ByteEnumerableExtensions.Format.OnlyAscii"/> is returned even if also IsOnlyHex is true.
        /// </summary>
        public static ByteEnumerableExtensions.Format OutputFormat
        {
            get
            {
                ByteEnumerableExtensions.Format result = ByteEnumerableExtensions.Format.Plain;

                if (GlobalParameters.IsOnlyAscii)
                {
                    result = ByteEnumerableExtensions.Format.OnlyAscii;
                }
                else if (GlobalParameters.IsOnlyHex)
                {
                    result = ByteEnumerableExtensions.Format.OnlyHex;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets or sets the name of the real port connected to the external device.
        /// </summary>
        public static string RealPort
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the baud rate to be used in the communications
        /// </summary>
        public static int TransmissionBaudRate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of data bits to be used in the serial transmission.
        /// </summary>
        public static int TransmissionDataBits
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the parity to be used in the serial communication.
        /// </summary>
        public static Parity TransmissionParity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the stop bits to be used in the serial communication.
        /// </summary>
        public static StopBits TransmissionStopBits
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the VirtualPort, i.e. the port managed by com0com and null-connected to another virtual port to which the software under test communicates.
        /// </summary>
        public static string VirtualPort
        {
            get;
            set;
        }

        /// <summary>
        /// Parses the passed command line parameters and populates the static properties used as global parameters of this application.
        /// </summary>
        /// <param name="args">String array containing the command line parameters.</param>
        public static void ParseCommandLineArguments(string[] args)
        {
            IsGui = false;
            CommandLineArgumentParser.ParseArguments(args);
            if (CommandLineArgumentParser.UniqueParameter != null)
            {
                if (CommandLineArgumentParser.UniqueParameter == "-gui")
                {
                    IsGui = true;
                }

                if (CommandLineArgumentParser.UniqueParameter == "-help")
                {
                    IsHelp = true;
                }
            }

            VirtualPort = CommandLineArgumentParser.GetParamValue("-virtual");
            RealPort = CommandLineArgumentParser.GetParamValue("-real");
            OutputFileName = CommandLineArgumentParser.GetParamValue("-output");
            TransmissionBaudRate = int.Parse(CommandLineArgumentParser.GetParamValue("-baud"));
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
                Value2StopBits);
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
            IsShowCollapsed = CommandLineArgumentParser.IsSwitchOn("-collapsed");
        }

        /// <summary>
        /// Saves the parameters to the Windows Registry. Used only in GUI mode
        /// </summary>
        public static void SaveToRegistry()
        {
            WindowsRegistry.Set(WindowsRegistryEntry.VIRTUALPORT, GlobalParameters.VirtualPort);
            WindowsRegistry.Set(WindowsRegistryEntry.REALPORT, GlobalParameters.RealPort);
            WindowsRegistry.Set(WindowsRegistryEntry.ISONLYHEX, GlobalParameters.IsOnlyHex);
            WindowsRegistry.Set(WindowsRegistryEntry.ISONLYASCII, GlobalParameters.IsOnlyAscii);
            WindowsRegistry.Set(WindowsRegistryEntry.ISCOLLAPSED, GlobalParameters.IsShowCollapsed);
            WindowsRegistry.Set(WindowsRegistryEntry.ISTIME, GlobalParameters.IsShowTime);
            WindowsRegistry.Set(WindowsRegistryEntry.BYTESPERLINE, GlobalParameters.BytesPerLine);
            WindowsRegistry.Set(WindowsRegistryEntry.BAUDRATE, GlobalParameters.TransmissionBaudRate.ToString());
            WindowsRegistry.Set(WindowsRegistryEntry.PARITY, GlobalParameters.TransmissionParity.ToString());
            WindowsRegistry.Set(WindowsRegistryEntry.STOPBIT, GlobalParameters.TransmissionStopBits.ToString());
            WindowsRegistry.Set(WindowsRegistryEntry.DATA, GlobalParameters.TransmissionDataBits.ToString());
        }
    }
}
