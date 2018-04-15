using Newtonsoft.Json;
using System;

namespace LilaSharp.Packets
{
    public class PHookIn : Packet
    {
        public override string Type { get {  return "hookIn"; } }

        [JsonProperty("d")]
        public object Data { get; private set; }

        public PHookIn() : base()
        {
            Data = new object();
        }
    }
}
