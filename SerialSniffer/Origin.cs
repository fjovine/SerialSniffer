//-----------------------------------------------------------------------
// <copyright file="Origin.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------
namespace SerialSniffer
{
    /// <summary>
    /// Enumerates the directions from where a packet can come from.
    /// </summary>
    public enum Origin
    {
        /// <summary>
        /// The packet comes from the real device
        /// </summary>
        FromReal,

        /// <summary>
        /// The packet comes from a simulated device, which is normally connected to a piece of software driving 
        /// a device.
        /// </summary>
        FromSimulated,

        /// <summary>
        /// The origin is (still) undefined.
        /// </summary>
        Undefined
    }
}
