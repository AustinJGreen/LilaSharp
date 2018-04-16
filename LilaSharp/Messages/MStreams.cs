using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message containing information about streams
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MStreams : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "streams";

        /// <summary>
        /// Gets or sets the HTML.
        /// </summary>
        /// <value>
        /// The HTML.
        /// </value>
        [JsonProperty("d")]
        public string Html { get; set; }
    }
}
