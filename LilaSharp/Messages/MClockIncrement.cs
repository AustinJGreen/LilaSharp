using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MClockIncrement : IGameMessage
    {
        public string Type => "clockInc";

        public int Version { get; set; }

        [JsonProperty("d")]
        public ClockIncrement Data { get; set; }

        
    }
}
