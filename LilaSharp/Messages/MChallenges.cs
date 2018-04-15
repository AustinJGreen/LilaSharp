using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MChallenges : IMessage
    {
        public string Type { get { return "challenges"; } }

        [JsonProperty("d")]
        public Challenges Challenges { get; set; }
    }
}
