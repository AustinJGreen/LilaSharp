using Newtonsoft.Json;

namespace LilaSharp.Packets
{
    /// <summary>
    /// Packet for watching games
    /// </summary>
    /// <seealso cref="LilaSharp.Packets.Packet" />
    public class PStartWatching : Packet
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("d")]
        public string Id { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PStartWatching"/> class.
        /// </summary>
        public PStartWatching(string id) : base("startWatching")
        {
            Id = id;
        }
    }
}
