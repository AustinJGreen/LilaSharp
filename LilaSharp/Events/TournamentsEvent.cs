using LilaSharp.Types;
using System.Collections.Generic;

namespace LilaSharp.Events
{
    /// <summary>
    /// Event for when tournament html data is received
    /// </summary>
    public class TournamentsEvent : LilaEvent
    {
        /// <summary>
        /// Gets the tournaments.
        /// </summary>
        /// <value>
        /// The tournaments.
        /// </value>
        public List<TournamentHtmlEntry> Tournaments { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentsEvent"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="tournaments">The tournaments.</param>
        public TournamentsEvent(LilaClient client, List<TournamentHtmlEntry> tournaments) : base(client)
        {
            Tournaments = tournaments;
        }
    }
}
