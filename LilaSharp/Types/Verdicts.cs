using Newtonsoft.Json;
using System.Collections.Generic;

namespace LilaSharp.Types
{
    public class Verdicts
    {
        [JsonProperty("list")]
        public List<object> List { get; set; }

        [JsonProperty("accepted")]
        public bool Accepted { get; set; }
    }
}
