using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MPong : IMessage
    {
        public string Type => "n";

        [JsonProperty("r")]
        public int Games { get; set; }

        [JsonProperty("d")]
        public int Players { get; set; }
    }
}
