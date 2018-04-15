using Newtonsoft.Json;
using System;

namespace LilaSharp.Packets
{
    public class PJoin : Packet
    {
        public override string Type {  get { return "join"; } }

        [JsonProperty("d")]
        public string Id { get; set; }

        public PJoin(string id) : base()
        {
            Id = id;
        }
    }
}
