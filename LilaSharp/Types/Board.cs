using Newtonsoft.Json;

namespace LilaSharp.Types
{
    /// <summary>
    /// Json Object containing information about a chessboard
    /// </summary>
    public class Board
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the variant.
        /// </summary>
        /// <value>
        /// The variant.
        /// </value>
        [JsonProperty("variant")]
        public Variant Variant { get; set; }

        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>
        /// The speed.
        /// </value>
        [JsonProperty("speed")]
        public string Speed { get; set; }

        /// <summary>
        /// Gets or sets the perf.
        /// </summary>
        /// <value>
        /// The perf.
        /// </value>
        [JsonProperty("perf")]
        public string Perf { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Board"/> is rated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if rated; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("rated")]
        public bool Rated { get; set; }

        /// <summary>
        /// Gets or sets the initial fen.
        /// </summary>
        /// <value>
        /// The initial fen.
        /// </value>
        [JsonProperty("initialFen")]
        public string InitialFen { get; set; }

        /// <summary>
        /// Gets or sets the fen.
        /// </summary>
        /// <value>
        /// The fen.
        /// </value>
        [JsonProperty("fen")]
        public string Fen { get; set; }

        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        /// <value>
        /// The player.
        /// </value>
        [JsonProperty("player")]
        public string Player { get; set; }

        /// <summary>
        /// Gets or sets the winner.
        /// </summary>
        /// <value>
        /// The winner.
        /// </value>
        [JsonProperty("winner")]
        public string Winner { get; set; }

        /// <summary>
        /// Gets or sets the turns played.
        /// </summary>
        /// <value>
        /// The turns.
        /// </value>
        [JsonProperty("turns")]
        public int Turns { get; set; }

        /// <summary>
        /// Gets or sets the started at turn.
        /// </summary>
        /// <value>
        /// The started at turn.
        /// </value>
        [JsonProperty("startedAtTurn")]
        public int StartedAtTurn { get; set; }

        /// <summary>
        /// Gets or sets the last move.
        /// </summary>
        /// <value>
        /// The last move.
        /// </value>
        [JsonProperty("lastMove")]
        public string LastMove { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        [JsonProperty("source")]
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [JsonProperty("status")]
        public Status Status { get; set; }

        /// <summary>
        /// Gets or sets the tournament identifier.
        /// </summary>
        /// <value>
        /// The tournament identifier.
        /// </value>
        [JsonProperty("tournamentId")]
        public string TournamentId { get; set; }

        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        /// <value>
        /// The created at.
        /// </value>
        [JsonProperty("createdAt")]
        public long CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the opening.
        /// </summary>
        /// <value>
        /// The opening.
        /// </value>
        [JsonProperty("opening")]
        public Opening Opening { get; set; }
    }
}
