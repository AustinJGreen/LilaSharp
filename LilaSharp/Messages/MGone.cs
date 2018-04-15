using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message for when opponent leaves or rejoins game
    /// </summary>
    public class MGone : ITypeMessage
    {
        public string Type => "gone";

        [JsonProperty("d")]
        public bool Left { get; set; }
    }
}
