//-----------------------------------------------------------------------
// <copyright file="WindowsRegistryEntry.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------
namespace SerialSniffer
{
    /// <summary>
    /// This class contains a list of strings for the registry entries.
    /// So registry string should be disseminated in the code: only strings coming
    /// from this class should be used.
    /// </summary>
    public static class WindowsRegistryEntry
    {
        /// <summary>
        /// Registry entry containing the name of the last virtual port used
        /// </summary>
        public const string VIRTUALPORT = "VirtualPort";

        /// <summary>
        /// Registry entry containing the name of the last folder opened.
        /// </summary>
        public const string REALPORT = "RealPort";

        /// <summary>
        /// Registry entry containing position and size of the main window.
        /// </summary>
        public const string STOREDWINDOWSPOSITION = "StoredWindowsPosition";

        /// <summary>
        /// Registry Key of the OutputFile property (GUI mode)
        /// </summary>
        public const string OUTPUTFILE = "OutputFile";

        /// <summary>
        /// Registry Key of the BaudRate property (GUI mode)
        /// </summary>
        public const string BAUDRATE = "BaudRate";

        /// <summary>
        /// Registry Key of the Parity property (GUI mode)
        /// </summary>
        public const string PARITY = "Parity";

        /// <summary>
        /// Registry Key of the <c>Stopbit</c> property (GUI mode)
        /// </summary>
        public const string STOPBIT = "Stopbit";

        /// <summary>
        /// Registry Key of the Data format property (GUI mode)
        /// </summary>
        public const string DATA = "Data";

        /// <summary>
        /// Registry Key of the IsOnlyHex property (GUI mode)
        /// </summary>
        public const string ISONLYHEX = "IsOnlyHex";

        /// <summary>
        /// Registry Key of the <c>IsOnlyAscii</c> property (GUI mode)
        /// </summary>
        public const string ISONLYASCII = "IsOnlyAscii";

        /// <summary>
        /// Registry Key of the IsTime property (GUI mode)
        /// </summary>
        public const string ISTIME = "IsTime";

        /// <summary>
        /// Registry Key of the BytesPerLine property (GUI mode)
        /// </summary>
        public const string BYTESPERLINE = "BytesPerLine";

        /// <summary>
        /// Registry Key of the IsCollapsed property (GUI mode)
        /// </summary>
        public const string ISCOLLAPSED = "IsCollapsed";
    }
}
