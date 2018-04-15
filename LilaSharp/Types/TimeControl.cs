using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class TimeControl
    {
        [JsonProperty("type")]
        public string type { get; set; }
    }
}
