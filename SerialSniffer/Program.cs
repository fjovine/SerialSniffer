//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------

namespace SerialSniffer
{
    using System;
    using System.IO.Ports;

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
            DateTime start = DateTime.MinValue;
            bool isFirst = true;

            Sniffer sniffer = new Sniffer("COM2", "COM3", 9600, Parity.None, StopBits.One, 8);
            sniffer.IsCollapsingSameOrigin = true;
            sniffer.Available += (s, e) =>
            {
                if (isFirst)
                {
                    start = e.When;
                    isFirst = false;
                }

                string preamble = string.Format(
                    "{0,10:0.000} {1} ",
                    e.When.Subtract(start).TotalMilliseconds,
                    e.Origin == Origin.FromReal ? '<' : '>');
                Console.WriteLine(e.Content.ToHex(preamble));
            };
            sniffer.OpenAndSniff();
            Console.Read();
        }
    }
}
