using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Watchers
    {
        [JsonProperty("nb")]
        public int Count { get; set; }

        [JsonProperty("users")]
        public string[] Users { get; set; }

        [JsonProperty("anons")]
        public int AnonymousCount { get; set; }
    }
}
