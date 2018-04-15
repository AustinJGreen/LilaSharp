namespace LilaSharp
{
    /// <summary>
    /// Class containing data for lichess moves.
    /// </summary>
    public class LilaMove
    {
        /// <summary>
        /// Gets or sets the role (for zh).
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public string To { get; set; }

        /// <summary>
        /// Gets or sets the promotion.
        /// </summary>
        /// <value>
        /// The promotion.
        /// </value>
        public string Promotion { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is drop.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is drop; otherwise, <c>false</c>.
        /// </value>
        public bool IsDrop { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return IsDrop? string.Format("{0}@{1}", Role, To) : string.Format("{0}{1}{2}", From, To, Promotion ?? "");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LilaMove"/> class with a crazyhouse drop move.
        /// </summary>
        public LilaMove(string role, string to)
        {
            Role = role;
            To = to;
            Promotion = null;
            IsDrop = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LilaMove"/> class.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="promotion">The promotion.</param>
        public LilaMove(string from, string to, string promotion)
        {
            From = from;
            To = to;
            Promotion = promotion;
            IsDrop = false;
        }
    }
}
