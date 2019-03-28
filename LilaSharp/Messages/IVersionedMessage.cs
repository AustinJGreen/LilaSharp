using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Interface for messages containing a version
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.IMessage" />
    public interface IVersionedMessage : IMessage
    {
        /// <summary>
        /// Gets or sets the version of this message
        /// </summary>
        [JsonProperty("v")]
        int Version { get; set; }
    }
}
