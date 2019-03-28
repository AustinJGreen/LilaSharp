using Newtonsoft.Json;

namespace LilaSharp.Types
{
    /// <summary>
    /// Clock increment data object
    /// </summary>
    public class ClockIncrement
    {
        /// <summary>
        /// Gets or sets the increment color
        /// </summary>
        [JsonProperty("color")]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the time incremented
        /// </summary>
        [JsonProperty("time")]
        public int Time { get; set; }
    }
}
