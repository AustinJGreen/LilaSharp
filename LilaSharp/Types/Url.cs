using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Url
    {
        [JsonProperty("socket")]
        public string Socket { get; set; }

        [JsonProperty("round")]
        public string Round { get; set; }
    }
}
