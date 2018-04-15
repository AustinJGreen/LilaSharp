using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class EndClock
    {
        [JsonProperty("wc")]
        public int WhiteClock { get; set; }

        [JsonProperty("bc")]
        public int BlackClock { get; set; }
    }
}
