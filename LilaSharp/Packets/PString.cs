namespace LilaSharp.Packets
{
    public class PString : Packet
    {
        private string type;

        public override string Type => type;

        public PString(string type)
        {
            this.type = type;
        }
    }
}
