using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Redirect
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
