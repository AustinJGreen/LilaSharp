using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MSimuls : ITypeMessage
    {
        public string Type => "simuls";

        [JsonProperty("d")]
        public string Html { get; set; }
    }
}
