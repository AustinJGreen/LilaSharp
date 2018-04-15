using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Status
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
