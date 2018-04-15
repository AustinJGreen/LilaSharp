using System;

namespace LilaSharp.Packets
{
    public class PPing : Packet
    {
        public override string Type { get { return "ping"; } }
    }
}
