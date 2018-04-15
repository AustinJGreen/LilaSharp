using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MCheckCount : ITypeMessage, IVersionedMessage
    {
        public int Version { get; set; }

        public string Type => "checkCount";

        [JsonProperty("d")]
        public MCheckCount Data { get; set; }
    }
}
