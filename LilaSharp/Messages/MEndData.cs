using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MEndData : ITypeMessage, IVersionedMessage
    {
        public int Version { get; set; }

        public string Type => "endData";

        [JsonProperty("d")]
        public MEndData Data { get; set; }
    }
}
