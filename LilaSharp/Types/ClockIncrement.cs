using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class ClockIncrement
    {
        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("time")]
        public int Time { get; set; }
    }
}
