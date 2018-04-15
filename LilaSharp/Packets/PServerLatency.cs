using Newtonsoft.Json;
namespace LilaSharp.Packets
{
    public class PServerLatency : Packet
    {
        public override string Type { get { return "moveLat"; } }

        [JsonProperty("d")]
        public bool Send { get; set; }

        public PServerLatency(bool send) : base()
        {
            Send = send;
        }
    }
}
