using Newtonsoft.Json;

namespace LilaSharp.Packets
{
    /// <summary>
    /// Packet for sending when the player idle
    /// </summary>
    /// <seealso cref="LilaSharp.Packets.Packet" />
    public class PIdle : Packet
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public override string Type {  get { return "idle"; } }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PIdle"/> is idle.
        /// </summary>
        /// <value>
        ///   <c>true</c> if idle; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("idle")]
        public bool Idle { get; set; }
    }
}
