using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class EndData
    {
        [JsonProperty("winner")]
        public string Winner { get; set; }

        [JsonProperty("status")]
        public EndStatus Status { get; set; }

        [JsonProperty("clock")]
        public EndClock Clock { get; set; }
    }
}
