using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class PlayerPerf
    {
        [JsonProperty("games")]
        public int Games { get; set; }

        [JsonProperty("rating")]
        public int Rating { get; set; }

        [JsonProperty("rd")]
        public int rd { get; set; }

        [JsonProperty("prov")]
        public bool prov { get; set; }
       
        [JsonProperty("prog")]
        public int prog { get; set; }
    }
}
