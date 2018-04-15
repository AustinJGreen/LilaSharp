using Newtonsoft.Json;
using System.Collections.Generic;

namespace LilaSharp.Types
{
    public class Sheet
    {
        [JsonProperty("scores")]
        public List<object> Scores { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("fire")]
        public bool Fire { get; set; }
    }
}
