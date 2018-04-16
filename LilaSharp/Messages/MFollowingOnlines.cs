using Newtonsoft.Json;
using System.Collections.Generic;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message containing information about players you follow becoming online
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MFollowingOnlines : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "following_onlines";

        /// <summary>
        /// Gets or sets the names of the players.
        /// </summary>
        /// <value>
        /// The names.
        /// </value>
        [JsonProperty("d")]
        public List<string> Names { get; set; }

        /// <summary>
        /// Gets or sets the playing of the players.
        /// </summary>
        /// <value>
        /// The playing.
        /// </value>
        [JsonProperty("playing")]
        public List<string> Playing { get; set; }

        /// <summary>
        /// Gets or sets the players who are studying.
        /// </summary>
        /// <value>
        /// The studying.
        /// </value>
        [JsonProperty("studying")]
        public List<string> Studying { get; set; }

        /// <summary>
        /// Gets or sets the players who are patrons.
        /// </summary>
        /// <value>
        /// The patrons.
        /// </value>
        [JsonProperty("patrons")]
        public List<string> Patrons { get; set; }
    }
}
