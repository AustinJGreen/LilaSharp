using Newtonsoft.Json;

namespace LilaSharp.Packets
{
    /// <summary>
    /// Packet for sending a chat message
    /// </summary>
    /// <seealso cref="LilaSharp.Packets.Packet" />
    public class PTalk : Packet
    {
        public override string Type => "talk";

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [JsonProperty("d")]
        public string Message { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PTalk"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public PTalk(string message)
        {
            Message = message;
        }
    }
}
