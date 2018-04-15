using Newtonsoft.Json;
using System.Collections.Generic;

namespace LilaSharp.Types
{
    public class ChatData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("lines")]
        public List<object> Lines { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("loginRequired")]
        public bool LoginRequired { get; set; }

        [JsonProperty("restricted")]
        public bool Restricted { get; set; }
    }
}
