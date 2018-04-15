using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Variant
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("short")]
        public string Short { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
