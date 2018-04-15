using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    interface IGameMessage : IMessage
    {
        [JsonProperty("v")]
        int Version { get; set; }
    }
}
