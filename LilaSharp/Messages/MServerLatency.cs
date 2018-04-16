using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message containing information about the server latency
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MServerLatency : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "mlat";

        /// <summary>
        /// Gets or sets the latency.
        /// </summary>
        /// <value>
        /// The latency.
        /// </value>
        [JsonProperty("d")]
        public int Latency { get; set; }
    }
}
