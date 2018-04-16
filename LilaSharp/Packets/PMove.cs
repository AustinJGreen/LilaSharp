using Newtonsoft.Json;

namespace LilaSharp.Packets
{
    /// <summary>
    /// Packet for sending move data to game
    /// </summary>
    /// <seealso cref="LilaSharp.Packets.Packet" />
    public class PMove : Packet
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public override string Type { get { return "move"; } }

        /// <summary>
        /// Gets or sets the uci.
        /// </summary>
        /// <value>
        /// The uci.
        /// </value>
        [JsonProperty("u")]
        public string Uci { get; set; }

        /// <summary>
        /// Gets or sets the promotion.
        /// </summary>
        /// <value>
        /// The promotion.
        /// </value>
        [JsonProperty("promotion", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Promotion { get; set; }

        /// <summary>
        /// Gets or sets the lag.
        /// </summary>
        /// <value>
        /// The lag.
        /// </value>
        [JsonProperty("l", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Lag { get; set; }
    }
}
