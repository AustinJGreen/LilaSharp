using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message containing data about the end of the game
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    /// <seealso cref="LilaSharp.Messages.IVersionedMessage" />
    public class MEndData : ITypeMessage, IVersionedMessage
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "endData";

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [JsonProperty("d")]
        public EndData Data { get; set; }
    }
}
