using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message containing information about players you follow who stopped playing
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MFollowingStoppedPlaying : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "following_stopped_playing";

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [JsonProperty("d")]
        public string Username { get; set; }
    }
}
