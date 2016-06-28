//-----------------------------------------------------------------------
// <copyright file="Utils.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------
namespace SerialSniffer
{
    using System;

    /// <summary>
    /// Contains some utility methods
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Maps a string value to the equivalent element in an enum.
        /// </summary>
        /// <typeparam name="T">Enum type to be used.</typeparam>
        /// <param name="value">String value to be mapped.</param>
        /// <returns>Item of the enum that has the same name as the passed string.</returns>
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
