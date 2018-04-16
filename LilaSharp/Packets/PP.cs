using System;
using Newtonsoft.Json.Linq;

namespace LilaSharp.Packets
{
    /// <summary>
    /// Packet for pinging sockets
    /// </summary>
    /// <seealso cref="LilaSharp.Packets.Packet" />
    public class PP : Packet
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public override string Type {  get { return "p"; } }
    }
}
