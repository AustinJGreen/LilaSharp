using Newtonsoft.Json;

namespace LilaSharp.Types
{
    /// <summary>
    /// Check count data for threecheck variant
    /// </summary>
    public class CheckCount
    {
        /// <summary>
        /// Gets or sets the amount of checks on white
        /// </summary>
        [JsonProperty("white")]
        public int ChecksOnWhite { get; set; }

        /// <summary>
        /// Gets or sets the amount of checks on black
        /// </summary>
        [JsonProperty("black")]
        public int ChecksOnBlack { get; set; }
    }
}
