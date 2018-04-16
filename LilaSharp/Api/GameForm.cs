namespace LilaSharp.API
{
    /// <summary>
    /// Information for creating a game
    /// </summary>
    public class GameForm
    {
        /// <summary>
        /// Gets or sets the variant.
        /// </summary>
        /// <value>
        /// The variant.
        /// </value>
        public int Variant { get; set; }

        /// <summary>
        /// Gets or sets the fen.
        /// </summary>
        /// <value>
        /// The fen.
        /// </value>
        public string Fen { get; set; }

        /// <summary>
        /// Gets or sets the time mode.
        /// </summary>
        /// <value>
        /// The time mode.
        /// </value>
        public int TimeMode { get; set; }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public double Time { get; set; }

        /// <summary>
        /// Gets or sets the increment.
        /// </summary>
        /// <value>
        /// The increment.
        /// </value>
        public int Increment { get; set; }

        /// <summary>
        /// Gets or sets the days.
        /// </summary>
        /// <value>
        /// The days.
        /// </value>
        public int Days { get; set; }

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>
        /// The mode.
        /// </value>
        public int Mode { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public string Color { get; set; }
    }
}
