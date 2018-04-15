using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Perf
    {
        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
