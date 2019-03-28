using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Challenger
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("title")]
        public object Title { get; set; }

        [JsonProperty("rating")]
        public int Rating { get; set; }

        [JsonProperty("provisional")]
        public bool Provisional { get; set; }

        [JsonProperty("online")]
        public bool Online { get; set; }

        [JsonProperty("lag")]
        public int Lag { get; set; }
    }
}
