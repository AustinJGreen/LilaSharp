using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MChallenges : ITypeMessage
    {
        public string Type { get { return "challenges"; } }

        [JsonProperty("d")]
        public ChallengesData Data { get; set; }
    }
}
