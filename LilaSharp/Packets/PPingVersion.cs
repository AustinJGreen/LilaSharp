using Newtonsoft.Json;

namespace LilaSharp.Packets
{
    /// <summary>
    /// Packet for pinging with version information
    /// </summary>
    /// <seealso cref="LilaSharp.Packets.Packet" />
    public class PPingVersion : Packet
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public override string Type { get { return "p"; } }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [JsonProperty("v")]
        public int Version { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PPingVersion"/> class.
        /// </summary>
        public PPingVersion() : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PPingVersion"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        public PPingVersion(int version) : base()
        {
            Version = version;
        }
    }
}
