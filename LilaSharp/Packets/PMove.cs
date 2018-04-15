using Newtonsoft.Json;
using System;

namespace LilaSharp.Packets
{
    public class PMove : Packet
    {
        public override string Type { get { return "move"; } }

        [JsonProperty("u")]
        public string Uci { get; set; }

        [JsonProperty("promotion", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Promotion { get; set; }

        [JsonProperty("l", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Lag { get; set; }
    }
}
