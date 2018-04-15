using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class CrowdData
    {
        [JsonProperty("black")]
        public bool Black { get; set; }

        [JsonProperty("white")]
        public bool White { get; set; }

        [JsonProperty("watchers")]
        public Watchers Watchers { get; set; }
    }
}
