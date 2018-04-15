using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class PlayerData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
