using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Featured
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("fen")]
        public string FEn { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("lastMove")]
        public string LastMove { get; set; }

        [JsonProperty("white")]
        public TournamentPlayer White { get; set; }

        [JsonProperty("black")]
        public TournamentPlayer Black { get; set; }
    }
}
