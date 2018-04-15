using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Hook : IHook
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("uid")]
        public string UID { get; set; }

        [JsonProperty("u")]
        public string Username { get; set; }

        [JsonProperty("rating")]
        public int Rating { get; set; }

        [JsonProperty("ra")]
        public bool Rated { get; set; }

        [JsonProperty("clock")]
        public string Clock { get; set; }

        [JsonProperty("t")]
        public int Time { get; set; }

        [JsonProperty("perf")]
        public string Variant { get; set; }

        [JsonProperty("s")]
        public Speed Speed { get; set; }
    }
}
