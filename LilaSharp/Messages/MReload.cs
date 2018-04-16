using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message indicating that information should be reloaded
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    /// <seealso cref="LilaSharp.Messages.IVersionedMessage" />
    public class MReload : ITypeMessage, IVersionedMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "reload";

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the reloaddata.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [JsonProperty("d")]
        public ReloadData Data { get; set; }
    }
}
