using Newtonsoft.Json;

namespace LilaSharp.Packets
{
    public class PPingVersion : Packet
    {
        public override string Type { get { return "p"; } }

        [JsonProperty("v")]
        public int Version { get; set; }

        public PPingVersion() : base()
        {

        }

        public PPingVersion(int version) : base()
        {
            Version = version;
        }
    }
}
