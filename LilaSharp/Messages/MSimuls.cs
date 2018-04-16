using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message containing information about simuls
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MSimuls : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "simuls";

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
