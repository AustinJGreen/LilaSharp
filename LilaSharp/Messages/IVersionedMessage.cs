using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public interface IVersionedMessage : IMessage
    {
        [JsonProperty("v")]
        int Version { get; set; }
    }
}
