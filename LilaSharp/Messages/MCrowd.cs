using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MCrowd : IMessage
    {
        public string Type => "crowd";

        [JsonProperty("d")]
        public CrowdData Data { get; set; }
    }
}
