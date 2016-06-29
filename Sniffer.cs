    //--------------------------------------------------------------------Serial---
// <copyright file="Sniffer.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------
namespace SerialSniffer
{
    using System;
    using System.Collections.Generic;
    using System.IO.Ports;

    /// <summary>
    /// Implements a serial line sniffer. <para/>
    /// The core concept is represented by the <see cref="http://com0com.sourceforge.net/"/> driver that creates a 
    /// couple of com ports that are connected one the other with a sort of virtual null modem: all that is sent to one 
    /// of these ports can be seen at the other one and vice versa.<para/>
    /// So this sniffer works as a relay that transmits all what it receives on one port to the other and vice versa.
    /// </summary>
    public class Sniffer
    {
        /// <summary>
        /// Real port that is connected to the external device.
        /// </summary>
        private SerialPort real;

        /// <summary>
        /// Simulated serial port. This is typically the port connected to the com0com virtual device, i.e.
        /// communicates with the host software.
        /// </summary>
        private SerialPort simulated;

        /// <summary>
        /// Origin where the last packet came from.
        /// </summary>
        private Origin lastOrigin = Origin.Undefined;

        /// <summary>
        /// Object used to synchronize the receiving process of packets. When data is being processed, this object
        /// is locked forcing incoming requests to wait for the previous packet processing to be finished.
        /// </summary>
        private object sync = new object();

        /// <summary>
        /// Buffer accumulating the bytes coming from packets having the same origin.
        /// </summary>
        private List<byte> bytesArrivedSameOrigin;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sniffer" /> class.
        /// </summary>
        /// <param name="simulatedPortName">Filename of the simulated port</param>
        /// <param name="realPortName">Filename of the real port</param>
        /// <param name="baudRate">Baud rate to be used.</param>
        /// <param name="parity">Parity of the communication to be used.</param>
        /// <param name="stopBits">Stop bits of the serial communication to be used.</param>
        /// <param name="dataBits">Number of data bit to be used.</param>
        public Sniffer(string simulatedPortName, string realPortName, int baudRate, Parity parity, StopBits stopBits, int dataBits)
        {
            this.simulated = new SerialPort(simulatedPortName);
            this.real = new SerialPort(realPortName);
            this.simulated.BaudRate = baudRate;
            this.simulated.Parity = parity;
            this.simulated.StopBits = stopBits;
            this.simulated.DataBits = dataBits;
            this.real.BaudRate = baudRate;
            this.real.Parity = parity;
            this.real.StopBits = stopBits;
            this.real.DataBits = dataBits;
            this.IsCollapsingSameOrigin = false;
        }

        /// <summary>
        /// Delegate used by the SniffedPacketAvailable event.
        /// </summary>
        /// <param name="sender">Sender object of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        public delegate void SniffedPacketAvailable(object sender, SniffedPacketEventArgs e);

        /// <summary>
        /// Event triggered whenever a new packet is available, coming from any ports.
        /// </summary>
        public event SniffedPacketAvailable PacketAvailable;

        /// <summary>
        /// Gets or sets a value indicating whether the arriving packets should be collapsed, i.e. that all the subsequent data coming from the same source is collapsed
        /// in only one packet. If this option is chosen, the sniffed data will contain one packet coming from the device and the following coming from the software, i.e.
        /// it is impossible to have two successive packets coming from the same origin.
        /// </summary>
        public bool IsCollapsingSameOrigin
        {
            get;
            set;
        }

        /// <summary>
        /// Delivers the SniffedPacketAvailable event.
        /// </summary>
        /// <param name="e">Class containing information about the packet being received.</param>
        public void OnSniffedPacketAvailable(SniffedPacketEventArgs e)
        {
            if (this.PacketAvailable != null)
            {
                this.PacketAvailable(this, e);
            }
        }

        /// <summary>
        /// Starts the process of sniffing.
        /// </summary>
        public void OpenAndSniff()
        {
            this.simulated.DataReceived += (s, e) =>
            {
                lock (sync)
                {
                    var packet = RelayAvailableData(simulated, real);
                    this.ManagePacket(Origin.FromSimulated, packet);
                }
            };

            this.real.DataReceived += (s, e) =>
            {
                lock (sync)
                {
                    var packet = RelayAvailableData(real, simulated);
                    this.ManagePacket(Origin.FromReal, packet);
                }
            };

            this.simulated.Open();
            this.real.Open();
        }
        
        /// <summary>
        /// This method receives all the packets with their origin and decides what to do (if collapse them or not) following the <c>IsCollapsingSameOrigin</c> property.
        /// </summary>
        /// <param name="origin">Where the packet comes from.</param>
        /// <param name="packet">Byte array forming the packet.</param>
        private void ManagePacket(Origin origin, byte[] packet)
        {
            if (this.IsCollapsingSameOrigin)
            {
                if (this.lastOrigin == Origin.Undefined)
                {
                    this.bytesArrivedSameOrigin = new List<byte>();
                    this.lastOrigin = origin;
                }
                else
                {
                    if (origin == this.lastOrigin)
                    {
                        this.bytesArrivedSameOrigin.AddRange(packet);
                    }
                    else
                    {
                        SniffedPacketEventArgs eventArgs = new SniffedPacketEventArgs(DateTime.Now, this.lastOrigin, this.bytesArrivedSameOrigin);
                        this.OnSniffedPacketAvailable(eventArgs);
                        this.bytesArrivedSameOrigin.Clear();
                        this.bytesArrivedSameOrigin.AddRange(packet);
                        this.lastOrigin = origin;
                    }
                }
            }
            else
            {
                SniffedPacketEventArgs eventArgs = new SniffedPacketEventArgs(DateTime.Now, origin, packet);
                this.OnSniffedPacketAvailable(eventArgs);
            }
        }

        /// <summary>
        /// Relays the available data coming from one of the ports to the other. Data is transparent: all the received
        /// bytes are sent.
        /// </summary>
        /// <param name="from">Serial port originating the packet.</param>
        /// <param name="to">Serial port receiving the packet.</param>
        /// <returns>The packet as a byte array.</returns>
        private byte[] RelayAvailableData(SerialPort from, SerialPort to)
        {
            /*
            int bytesAvailable = from.BytesToRead;
            byte[] result = new byte[bytesAvailable];
            from.Read(result, 0, bytesAvailable);
            */

            byte[] receiveBuffer = new byte[from.ReadBufferSize];
            int bytesRead = from.Read(receiveBuffer, 0, receiveBuffer.Length);
            byte[] result = new byte[bytesRead];

            Array.Copy(receiveBuffer, result, bytesRead);
            //            to.Write(result, 0, bytesAvailable);
            to.Write(result, 0, bytesRead);
            return result;
        }
    }
}
