using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MFollowingPlaying : ITypeMessage
    {
        public string Type => "following_playing";

        [JsonProperty("d")]
        public string Username { get; set; }
    }
}
