using Newtonsoft.Json;
using System;

namespace LilaSharp.Packets
{
    public abstract class Packet
    {
        [JsonProperty("t")]
        public virtual string Type { get; }

        [JsonIgnore]
        public DateTime LastSent { get; set; }

        [JsonIgnore]
        public bool ShouldSerializeType { get; set; }

        public Packet DataClone()
        {
            Packet p = (Packet)MemberwiseClone();
            p.ShouldSerializeType = false;
            return p;
        }

        public Packet()
        {
            ShouldSerializeType = true;
        }
    }
}
