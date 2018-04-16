using Newtonsoft.Json;

namespace LilaSharp.Types
{
    /// <summary>
    /// Type containing information about a hook/seek
    /// </summary>
    /// <seealso cref="LilaSharp.Types.IHook" />
    public class Hook : IHook
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
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        [JsonProperty("uid")]
        public string UniqueId { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [JsonProperty("u")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        /// <value>
        /// The rating.
        /// </value>
        [JsonProperty("rating")]
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Hook"/> is rated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if rated; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("ra")]
        public bool Rated { get; set; }

        /// <summary>
        /// Gets or sets the clock.
        /// </summary>
        /// <value>
        /// The clock.
        /// </value>
        [JsonProperty("clock")]
        public string Clock { get; set; }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        [JsonProperty("t")]
        public int Time { get; set; }

        /// <summary>
        /// Gets or sets the variant.
        /// </summary>
        /// <value>
        /// The variant.
        /// </value>
        [JsonProperty("perf")]
        public string Variant { get; set; }

        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>
        /// The speed.
        /// </value>
        [JsonProperty("s")]
        public Speed Speed { get; set; }
    }
}
