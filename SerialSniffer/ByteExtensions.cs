//-----------------------------------------------------------------------
// <copyright file="ByteExtensions.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------
namespace SerialSniffer
{
    /// <summary>
    /// Extension methods for human readable output of a single byte as hex string. 
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// Converts the byte as an ASCII Character. It returns the character if printable, dot if not.
        /// </summary>
        /// <param name="theByte">Byte to be printed.</param>
        /// <returns>The corresponding ASCII character if printable, dot if not.</returns>
        public static char PrintPrintableOrPoint(this byte theByte)
        {
            if (theByte >= 32 && theByte <= 0x7E)
            {
                return (char)theByte;
            }
            else
            {
                return '.';
            }
        }

        /// <summary>
        /// Extension method that converts the byte in a two digit hex number.
        /// </summary>
        /// <param name="theByte">Byte to be converted.</param>
        /// <returns>Converted string.</returns>
        public static string ToHex(this byte theByte)
        {
            if (theByte < 16)
            {
                return "0" + theByte.ToString("x");
            }
            else
            {
                return theByte.ToString("x");
            }
        }
    }
}
