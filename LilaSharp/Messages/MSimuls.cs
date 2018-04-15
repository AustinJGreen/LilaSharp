using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MSimuls : IMessage
    {
        public string Type => "simuls";

        [JsonProperty("d")]
        public string Html { get; set; }
    }
}
