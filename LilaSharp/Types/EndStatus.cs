using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class EndStatus
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
