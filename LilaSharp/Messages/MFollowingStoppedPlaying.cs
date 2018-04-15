using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MFollowingStoppedPlaying : IMessage
    {
        public string Type => "following_stopped_playing";

        [JsonProperty("d")]
        public string Username { get; set; }
    }
}
