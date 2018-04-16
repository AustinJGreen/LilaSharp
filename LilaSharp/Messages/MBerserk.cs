using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message signaliing that a player has berserked
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    /// <seealso cref="LilaSharp.Messages.IVersionedMessage" />
    public class MBerserk : ITypeMessage, IVersionedMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "berserk";

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the color of the player.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        [JsonProperty("d")]
        public string Color { get; set; }        
    }
}
