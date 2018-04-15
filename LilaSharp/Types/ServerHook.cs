using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class ServerHook : IHook
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("rating")]
        public int Rating { get; set; }

        [JsonProperty("variant")]
        public Variant Variant { get; set; }

        [JsonProperty("mode")]
        public int Mode { get; set; }

        [JsonProperty("days")]
        public int? Days { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("perf")]
        public Perf Perf { get; set; }
    }
}
