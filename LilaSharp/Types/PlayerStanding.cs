using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class PlayerStanding
    {
        [JsonProperty("n")]
        public string Name { get; set; }

        [JsonProperty("s")]
        public int Score { get; set; }

        [JsonProperty("f")]
        public bool Fire { get; set; }

        [JsonProperty("t")]
        public string Title { get; set; }

        [JsonProperty("w")]
        public bool Withdrawn { get; set; }
    }
}
