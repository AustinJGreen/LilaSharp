using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MReload : ITypeMessage, IVersionedMessage
    {
        public string Type => "reload";

        public int Version { get; set; }

        [JsonProperty("d")]
        public ReloadData Data { get; set; }
    }
}
