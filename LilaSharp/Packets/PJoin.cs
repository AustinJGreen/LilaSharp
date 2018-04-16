using Newtonsoft.Json;
using System;

namespace LilaSharp.Packets
{
    /// <summary>
    /// Packet for joining a seek/hook
    /// </summary>
    /// <seealso cref="LilaSharp.Packets.Packet" />
    public class PJoin : Packet
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public override string Type {  get { return "join"; } }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("d")]
        public string Id { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PJoin"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public PJoin(string id) : base()
        {
            Id = id;
        }
    }
}
