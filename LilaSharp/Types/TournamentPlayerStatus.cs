using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class TournamentPlayerStatus
    {
        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("withdraw")]
        public bool Withdrawn { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}
