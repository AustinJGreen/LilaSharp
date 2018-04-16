using Newtonsoft.Json;

namespace LilaSharp.Types
{
    /// <summary>
    /// Json Object containing information about a challenge
    /// </summary>
    public class Challenge
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
        /// Gets or sets the direction. (in or out)
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        [JsonProperty("direction")]
        public string Direction { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the challenger.
        /// </summary>
        /// <value>
        /// The challenger.
        /// </value>
        [JsonProperty("challenger")]
        public Challenger Challenger { get; set; }

        /// <summary>
        /// Gets or sets the dest user.
        /// </summary>
        /// <value>
        /// The dest user.
        /// </value>
        [JsonProperty("destUser")]
        public object DestUser { get; set; }

        /// <summary>
        /// Gets or sets the variant.
        /// </summary>
        /// <value>
        /// The variant.
        /// </value>
        [JsonProperty("variant")]
        public Variant Variant { get; set; }

        /// <summary>
        /// Gets or sets the initial fen.
        /// </summary>
        /// <value>
        /// The initial fen.
        /// </value>
        [JsonProperty("initialFen")]
        public object InitialFen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Challenge"/> is rated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if rated; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("rated")]
        public bool Rated { get; set; }

        /// <summary>
        /// Gets or sets the time control.
        /// </summary>
        /// <value>
        /// The time control.
        /// </value>
        [JsonProperty("timeControl")]
        public TimeControl TimeControl { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        [JsonProperty("color")]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the perf.
        /// </summary>
        /// <value>
        /// The perf.
        /// </value>
        [JsonProperty("perf")]
        public Perf Perf { get; set; }
    }
}
