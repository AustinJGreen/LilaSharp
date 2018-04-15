using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MCheckCount : IGameMessage
    {
        public int Version { get; set; }

        public string Type => "checkCount";

        [JsonProperty("d")]
        public CheckCount CheckCount { get; set; }
    }
}
