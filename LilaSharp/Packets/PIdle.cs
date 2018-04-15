using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace LilaSharp.Packets
{
    public class PIdle : Packet
    {
        public override string Type {  get { return "idle"; } }

        [JsonProperty("idle")]
        public bool Idle { get; set; }
    }
}
