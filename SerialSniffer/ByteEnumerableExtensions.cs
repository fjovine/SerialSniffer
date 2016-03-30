//-----------------------------------------------------------------------
// <copyright file="ByteEnumerableExtensions.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------
namespace SerialSniffer
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Printing extension methods to <c>enumerables</c> of bytes.
    /// </summary>
    public static class ByteEnumerableExtensions
    {
        /// <summary>
        /// Enumerates the possible format options.
        /// </summary>
        public enum Format
        {
            /// <summary>
            /// Both the hex decoded bytes and the ASCII counterparts are shown on the same line
            /// </summary>
            Plain,

            /// <summary>
            /// Only hex data is shown
            /// </summary>
            OnlyHex,

            /// <summary>
            /// Only ASCII encoding is shown with a dot for non printable characters
            /// </summary>
            OnlyAscii
        }

        /// <summary>
        /// Prints the content of the enumeration on the output string in two columns: the leftmost is for 
        /// hex representation of data, the rightmost is for ASCII representation with . for non printable
        /// characters. A similar format to the <c>Hexdump</c> utility.
        /// Optionally, third column to the left is present where the preamble string is shown. If the content
        /// is longer than one row, only the first row will contain the preamble, the following rows will contain
        /// blanks.
        /// </summary>
        /// <param name="theEnumeration">The byte enumeration to be printed.</param>
        /// <param name="preamble">Preamble string, see the description of the method.</param>
        /// <param name="format">Format to be used generating the output.</param>
        /// <param name="bytesPerRow">Number of bytes per row. The default is 16.</param>
        /// <returns>A string formatted as described by the method description.</returns>
        public static string ToHex(this IEnumerable<byte> theEnumeration, string preamble, Format format = Format.Plain, int bytesPerRow = 16)
        {
            string trailingSpaces = new string(' ', preamble.Length);
            int elementsCount = theEnumeration.Count<byte>();
            int rowsCount = elementsCount / bytesPerRow;
            if ((elementsCount % bytesPerRow) > 0)
            {
                rowsCount++;
            }

            StringBuilder result = new StringBuilder(rowsCount * (preamble.Length + 4 + (4 * bytesPerRow)));
            StringBuilder hexPart = new StringBuilder(3 * bytesPerRow);
            StringBuilder asciiPart = new StringBuilder(bytesPerRow);
            int pos = 0;
            int totalCounter = 0;
            bool isFirst = true;
            foreach (byte by in theEnumeration)
            {
                hexPart.Append(by.ToHex());
                hexPart.Append(' ');
                asciiPart.Append(by.PrintPrintableOrPoint());
                totalCounter++;
                if (++pos >= bytesPerRow || totalCounter >= elementsCount)
                {
                    // new Row
                    if (isFirst)
                    {
                        result.Append(preamble);
                        isFirst = false;
                    }
                    else
                    {
                        result.Append('\n');
                        result.Append(trailingSpaces);
                    }

                    if (format != Format.OnlyAscii)
                    {
                        result.Append(hexPart);
                    }

                    if (format != Format.OnlyHex)
                    {
                        if (format == Format.Plain)
                        {
                            result.Append(' ', 3 * (bytesPerRow - pos));
                            result.Append("| ");
                        }

                        result.Append(asciiPart);
                    }

                    pos = 0;
                    hexPart.Clear();
                    asciiPart.Clear();
                }
            }

            return result.ToString();
        }
    }
}
