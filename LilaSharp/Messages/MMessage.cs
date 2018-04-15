using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MMessage : ITypeMessage, IVersionedMessage
    {
        public string Type => "message";

        public int Version { get; set; }

        [JsonProperty("d")]
        public MessageData Data { get; set; }
    }
}
