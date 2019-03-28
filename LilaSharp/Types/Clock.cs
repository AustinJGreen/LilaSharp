using Newtonsoft.Json;

namespace LilaSharp.Types
{
    /// <summary>
    /// Clock data object
    /// </summary>
    public class Clock
    {
        /// <summary>
        /// Gets or sets if the clock is running
        /// </summary>
        [JsonProperty("running")]
        public bool Running { get; set; }

        /// <summary>
        /// Gets or sets the initial clock time
        /// </summary>
        [JsonProperty("initial")]
        public int Initial { get; set; }

        /// <summary>
        /// Gets or sets the clock increment
        /// </summary>
        [JsonProperty("increment")]
        public int Increment { get; set; }

        /// <summary>
        /// Gets or sets the time left for white
        /// </summary>
        [JsonProperty("white")]
        public double White { get; set; }

        /// <summary>
        /// Gets or sets the time left for black
        /// </summary>
        [JsonProperty("black")]
        public double Black { get; set; }

        /// <summary>
        /// Gets or sets the emergency time
        /// </summary>
        [JsonProperty("emerg")]
        public int Emerg { get; set; }

        /// <summary>
        /// Gets or sets more time given
        /// </summary>
        [JsonProperty("moretime")]
        public int MoreTime { get; set; }
    }
}
