using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message for when opponent leaves or rejoins game
    /// </summary>
    public class MGone : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "gone";

        /// <summary>
        /// Gets or sets a value indicating whether the player left.
        /// </summary>
        /// <value>
        ///   <c>true</c> if left; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("d")]
        public bool Left { get; set; }
    }
}
