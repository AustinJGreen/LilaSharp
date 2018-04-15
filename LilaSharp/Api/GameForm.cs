namespace LilaSharp.Api
{
    //TODO: change  Variant, Color to enums
    public class GameForm
    {
        public int Variant { get; set; }

        public string Fen { get; set; }

        public int TimeMode { get; set; }

        public double Time { get; set; }

        public int Increment { get; set; }

        public int Days { get; set; }

        public int Mode { get; set; }

        public string Color { get; set; }
    }
}
