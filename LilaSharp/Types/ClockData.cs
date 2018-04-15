using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class ClockData
    {
        [JsonProperty("black")]
        public double Black { get; set; }

        [JsonProperty("white")]
        public double White { get; set; }

        [JsonProperty("lag")]
        public int Lag { get; set; }
    }
}
