using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message containing information about challenges received
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MChallenges : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get { return "challenges"; } }

        /// <summary>
        /// Gets or sets the challenges data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [JsonProperty("d")]
        public ChallengesData Data { get; set; }
    }
}
