using Newtonsoft.Json;

namespace LilaSharp.Types
{
    /// <summary>
    /// Clock data object
    /// </summary>
    public class ClockData
    {
        /// <summary>
        /// Gets or sets time left for black
        /// </summary>
        [JsonProperty("black")]
        public double Black { get; set; }

        /// <summary>
        /// Gets or sets time left for white
        /// </summary>
        [JsonProperty("white")]
        public double White { get; set; }

        /// <summary>
        /// Gets or sets the clock lag
        /// </summary>
        [JsonProperty("lag")]
        public int Lag { get; set; }
    }
}
