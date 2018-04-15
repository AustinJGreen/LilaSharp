using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Challenger
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("title")]
        public object title { get; set; }

        [JsonProperty("rating")]
        public int rating { get; set; }

        [JsonProperty("provisional")]
        public bool provisional { get; set; }

        [JsonProperty("online")]
        public bool online { get; set; }

        [JsonProperty("lag")]
        public int lag { get; set; }
    }
}
