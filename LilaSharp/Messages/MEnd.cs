using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message indicating the end of a game
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    /// <seealso cref="LilaSharp.Messages.IVersionedMessage" />
    public class MEnd : ITypeMessage, IVersionedMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "end";

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the color of the winner.
        /// </summary>
        /// <value>
        /// The color of the winner.
        /// </value>
        [JsonProperty("d")]
        public string WinnerColor { get; set; }
    }
}
