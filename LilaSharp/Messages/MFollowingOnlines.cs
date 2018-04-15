using Newtonsoft.Json;
using System.Collections.Generic;

namespace LilaSharp.Messages
{
    public class MFollowingOnlines : ITypeMessage
    {
        public string Type => "following_onlines";

        [JsonProperty("d")]
        public List<string> Names { get; set; }

        [JsonProperty("playing")]
        public List<string> Playing { get; set; }

        [JsonProperty("studying")]
        public List<string> Studying { get; set; }

        [JsonProperty("patrons")]
        public List<string> Patrons { get; set; }
    }
}
