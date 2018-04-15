namespace LilaSharp.Api
{
    public class Seek
    {
        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public int Time { get; set; }

        /// <summary>
        /// Gets or sets the increment.
        /// </summary>
        /// <value>
        /// The increment.
        /// </value>
        public int Increment { get; set; }

        /// <summary>
        /// Gets or sets the minimum rating.
        /// </summary>
        /// <value>
        /// The minimum rating.
        /// </value>
        public int MinRating { get; set; }

        /// <summary>
        /// Gets or sets the maximum rating.
        /// </summary>
        /// <value>
        /// The maximum rating.
        /// </value>
        public int MaxRating { get; set; }
    }
}
