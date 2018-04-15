using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class ReloadData
    {
        [JsonProperty("t")]
        public string Type { get; set; }

        [JsonProperty("d")]
        public string Data { get; set; }
    }
}
