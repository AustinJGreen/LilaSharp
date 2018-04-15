using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class ReminderData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
