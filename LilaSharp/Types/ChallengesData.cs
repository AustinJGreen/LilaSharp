using Newtonsoft.Json;
using System.Collections.Generic;

namespace LilaSharp.Types
{
    /// <summary>
    /// Challenges data object
    /// </summary>
    public class ChallengesData
    {
        /// <summary>
        /// Gets or sets incoming challenges
        /// </summary>
        [JsonProperty("in")]
        public List<Challenge> In { get; set; }

        /// <summary>
        /// Gets or sets outgoing challenges
        /// </summary>
        [JsonProperty("out")]
        public List<Challenge> Out { get; set; }

        /// <summary>
        /// Gets or sets the internationalization
        /// </summary>
        [JsonProperty("i18n")]
        public I18n Il8n { get; set; }
    }
}
