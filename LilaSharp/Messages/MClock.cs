using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MClock : ITypeMessage, IVersionedMessage
    {
        public int Version { get; set; }

        public string Type => "clock";

        [JsonProperty("d")]
        public ClockData Data { get; set; }
    }
}
