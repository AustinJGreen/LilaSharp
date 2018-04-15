using System;
using Newtonsoft.Json.Linq;

namespace LilaSharp.Packets
{
    public class PP : Packet
    {
        public override string Type {  get { return "p"; } }
    }
}
