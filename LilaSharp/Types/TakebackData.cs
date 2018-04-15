using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class TakebackData
    {
        [JsonProperty("black")]
        public bool Black { get; set; }

        [JsonProperty("white")]
        public bool White { get; set; }
    }
}
