using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MStreams : IMessage
    {
        public string Type => "streams";

        [JsonProperty("d")]
        public string Html { get; set; }
    }
}
