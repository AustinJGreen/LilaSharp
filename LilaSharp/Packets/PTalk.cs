using Newtonsoft.Json;

namespace LilaSharp.Packets
{
    public class PTalk : Packet
    {
        public override string Type => "talk";

        [JsonProperty("d")]
        public string Message { get; set; }

        public PTalk(string message)
        {
            Message = message;
        }
    }
}
