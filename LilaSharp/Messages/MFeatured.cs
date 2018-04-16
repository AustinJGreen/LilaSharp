using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message containing data about featured events on lichess
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MFeatured : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "featured";

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [JsonProperty("d")]
        public FeaturedData Data { get; set; }
    }
}
