using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MBerserk : ITypeMessage, IVersionedMessage
    {
        public string Type => "berserk";

        public int Version { get; set; }

        [JsonProperty("d")]
        public string Color { get; set; }        
    }
}
