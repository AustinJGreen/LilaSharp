using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Clock
    {
        [JsonProperty("running")]
        public bool Running { get; set; }

        [JsonProperty("initial")]
        public int Initial { get; set; }

        [JsonProperty("increment")]
        public int Increment { get; set; }

        [JsonProperty("white")]
        public double White { get; set; }

        [JsonProperty("black")]
        public double Black { get; set; }

        [JsonProperty("emerg")]
        public int Emerg { get; set; }

        [JsonProperty("moretime")]
        public int MoreTime { get; set; }
    }
}
