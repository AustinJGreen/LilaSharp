using Newtonsoft.Json;
using System;

namespace LilaSharp.Packets
{
    /// <summary>
    /// Packet for requesting to receive hooks/seeks
    /// </summary>
    /// <seealso cref="LilaSharp.Packets.Packet" />
    public class PHookIn : Packet
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public override string Type { get {  return "hookIn"; } }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [JsonProperty("d")]
        public object Data { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PHookIn"/> class.
        /// </summary>
        public PHookIn() : base()
        {
            Data = new object();
        }
    }
}
