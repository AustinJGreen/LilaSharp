using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MClockIncrement : ITypeMessage, IVersionedMessage
    {
        public string Type => "clockInc";

        public int Version { get; set; }

        [JsonProperty("d")]
        public MClockIncrement Data { get; set; }

        
    }
}
