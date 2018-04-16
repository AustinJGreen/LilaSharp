using System;

namespace LilaSharp.Packets
{
    /// <summary>
    /// Packet for sending pings in the challenge socket.
    /// </summary>
    /// <seealso cref="LilaSharp.Packets.Packet" />
    public class PPing : Packet
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public override string Type { get { return "ping"; } }
    }
}
