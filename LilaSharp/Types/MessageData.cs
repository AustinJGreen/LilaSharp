using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class MessageData
    {
        [JsonProperty("u")]
        public string Username { get; set; }

        [JsonProperty("t")]
        public string Text { get; set; }
    }
}
