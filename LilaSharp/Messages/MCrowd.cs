using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message containing information about users spectating a game
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MCrowd : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "crowd";

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [JsonProperty("d")]
        public CrowdData Data { get; set; }
    }
}
