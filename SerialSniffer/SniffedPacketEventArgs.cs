//-----------------------------------------------------------------------
// <copyright file="SniffedPacketEventArgs.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------
namespace SerialSniffer
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Arguments of the SniffedPacketEvent that carries information about the packet that has been relayed.<para/>
    /// The information contained is
    /// <list type="">
    /// <item>Time when the packet arrived</item>
    /// <item>The direction of the packet: if it goes or if it comes.</item>
    /// <item>The content represented as an enumeration of bytes.</item>
    /// </list>
    /// </summary>
    public class SniffedPacketEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SniffedPacketEventArgs" /> class.
        /// </summary>
        /// <param name="when">Date and time when the packet arrived.</param>
        /// <param name="origin">The origin of the packet.</param>
        /// <param name="content">Enumeration of the byte forming the packet.</param>
        public SniffedPacketEventArgs(DateTime when, Origin origin, IEnumerable<byte> content)
        {
            this.When = when;
            this.Origin = origin;
            this.Content = content;
        }

        /// <summary>
        /// Gets when the packet has been received.
        /// </summary>
        public DateTime When
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets where the packet comes from.
        /// </summary>
        public Origin Origin
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the content of the packet as an enumerable of bytes.
        /// </summary>
        public IEnumerable<byte> Content
        {
            get;
            private set;
        }
    }
}
