using Newtonsoft.Json;
using System.Collections.Generic;

namespace LilaSharp.Types
{
    public class ChallengesData
    {
        [JsonProperty("in")]
        public List<Challenge> In { get; set; }

        [JsonProperty("out")]
        public List<Challenge> Out { get; set; }

        [JsonProperty("i18n")]
        public I18n Il8n { get; set; }
    }
}
