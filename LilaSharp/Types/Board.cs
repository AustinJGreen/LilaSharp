using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Board
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("variant")]
        public Variant Variant { get; set; }

        [JsonProperty("speed")]
        public string Speed { get; set; }

        [JsonProperty("perf")]
        public string Perf { get; set; }

        [JsonProperty("rated")]
        public bool Rated { get; set; }

        [JsonProperty("initialFen")]
        public string InitialFen { get; set; }

        [JsonProperty("fen")]
        public string Fen { get; set; }

        [JsonProperty("player")]
        public string Player { get; set; }

        [JsonProperty("winner")]
        public string Winner { get; set; }

        [JsonProperty("turns")]
        public int Turns { get; set; }

        [JsonProperty("startedAtTurn")]
        public int StartedAtTurn { get; set; }

        [JsonProperty("lastMove")]
        public string LastMove { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("tournamentId")]
        public string TournamentId { get; set; }

        [JsonProperty("createdAt")]
        public long CreatedAt { get; set; }

        [JsonProperty("opening")]
        public Opening Opening { get; set; }
    }
}
