using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Permissions
    {
        [JsonProperty("local")]
        public bool Local { get; set; }
    }
}
