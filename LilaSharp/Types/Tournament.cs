using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Tournament
    {
        [JsonProperty("data")]
        public TournamentData Data { get; set; }

        [JsonProperty("i8ln")]
        public I18n I18n { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("chat")]
        public Chat Chat { get; set; }
    }
}
