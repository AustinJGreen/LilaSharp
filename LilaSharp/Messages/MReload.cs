using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MReload : IGameMessage
    {
        public string Type => "reload";

        public int Version { get; set; }

        [JsonProperty("d")]
        public ReloadData Data { get; set; }
    }
}
