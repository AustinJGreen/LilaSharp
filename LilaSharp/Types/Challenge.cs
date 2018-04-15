using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Challenge
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("direction")]
        public string direction { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("challenger")]
        public Challenger challenger { get; set; }

        [JsonProperty("destUser")]
        public object destUser { get; set; }

        [JsonProperty("variant")]
        public Variant variant { get; set; }

        [JsonProperty("initialFen")]
        public object initialFen { get; set; }

        [JsonProperty("rated")]
        public bool rated { get; set; }

        [JsonProperty("timeControl")]
        public TimeControl timeControl { get; set; }

        [JsonProperty("color")]
        public string color { get; set; }

        [JsonProperty("perf")]
        public Perf perf { get; set; }
    }
}
