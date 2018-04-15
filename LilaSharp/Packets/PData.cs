using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilaSharp.Packets
{
    public class PData<P> : Packet where P : Packet
    {
        private P data;

        public override string Type
        {
            get
            {
                return data.Type;
            }
        }

        [JsonProperty("d")]
        public P Data { get { return data; } }

        public PData(P data) : base()
        {
            this.data = (P)data.DataClone();
        }
    }
}
