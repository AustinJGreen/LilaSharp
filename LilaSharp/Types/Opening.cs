using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Opening
    {
        [JsonProperty("eco")]
        public string Eco { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("ply")]
        public int Ply { get; set; }
    }
}
