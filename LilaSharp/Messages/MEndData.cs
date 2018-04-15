using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MEndData : IGameMessage
    {
        public int Version { get; set; }

        public string Type => "endData";

        [JsonProperty("d")]
        public EndData Data { get; set; }
    }
}
