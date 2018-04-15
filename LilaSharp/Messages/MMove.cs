using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MMove : IGameMessage
    {
        public int Version { get; set; }

        public string Type { get { return "move"; } }

        [JsonProperty("d")]
        public MoveData Data { get; set; }
    }
}
