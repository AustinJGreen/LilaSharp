using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Interface for messages containing a version
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.IMessage" />
    public interface IVersionedMessage : IMessage
    {
        [JsonProperty("v")]
        int Version { get; set; }
    }
}
