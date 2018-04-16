using Newtonsoft.Json;
namespace LilaSharp.Packets
{
    /// <summary>
    /// Packet for requesting server latency information
    /// </summary>
    /// <seealso cref="LilaSharp.Packets.Packet" />
    public class PServerLatency : Packet
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public override string Type { get { return "moveLat"; } }

        /// <summary>
        /// Gets or sets a value indicating whether lichess should send lag information.
        /// </summary>
        /// <value>
        ///   <c>true</c> if send; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("d")]
        public bool Send { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PServerLatency"/> class.
        /// </summary>
        /// <param name="send">if set to <c>true</c> [send].</param>
        public PServerLatency(bool send) : base()
        {
            Send = send;
        }
    }
}
