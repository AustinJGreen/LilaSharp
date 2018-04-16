using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MPong : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "n";

        /// <summary>
        /// Gets or sets the games.
        /// </summary>
        /// <value>
        /// The games.
        /// </value>
        [JsonProperty("r")]
        public int Games { get; set; }

        /// <summary>
        /// Gets or sets the players.
        /// </summary>
        /// <value>
        /// The players.
        /// </value>
        [JsonProperty("d")]
        public int Players { get; set; }
    }
}
