using Newtonsoft.Json;

namespace LilaSharp.Api
{
    public class HookResult
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
