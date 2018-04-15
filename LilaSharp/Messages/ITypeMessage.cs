using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public interface ITypeMessage : IMessage
    {
        [JsonProperty("t")]
        string Type { get; }
    }
}
