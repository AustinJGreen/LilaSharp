namespace LilaSharp.Events
{
    /// <summary>
    /// Event for when tournament data is received
    /// </summary>
    public class TournamentEvent : LilaEvent
    {
        /// <summary>
        /// Gets the tournament.
        /// </summary>
        /// <value>
        /// The tournament.
        /// </value>
        public LilaTournament Tournament { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentEvent"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="tournament">The tournament.</param>
        public TournamentEvent(LilaClient client, LilaTournament tournament) : base(client)
        {
            Tournament = tournament;
        }
    }
}
