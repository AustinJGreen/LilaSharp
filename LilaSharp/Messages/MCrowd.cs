using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MCrowd : ITypeMessage
    {
        public string Type => "crowd";

        [JsonProperty("d")]
        public CrowdData Data { get; set; }
    }
}
