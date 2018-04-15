using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Player
    {
        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("rating")]
        public int Rating { get; set; }

        [JsonProperty("provisional")]
        public bool Provisional { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("onGame")]
        public bool OnGame { get; set; }
    }
}
