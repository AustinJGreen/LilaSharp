using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class FeaturedData
    {
        [JsonProperty("html")]
        public string Html { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
