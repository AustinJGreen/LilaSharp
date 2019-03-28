using LilaSharp.Types;

namespace LilaSharp.Events
{
    /// <summary>
    /// Event data for challenges
    /// </summary>
    public class ChallengesEvent : LilaEvent
    {
        /// <summary>
        /// Gets or sets the challenges.
        /// </summary>
        /// <value>
        /// The challenges.
        /// </value>
        public ChallengesData Challenges { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChallengesEvent"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="challenges">The challenges.</param>
        public ChallengesEvent(LilaClient client, ChallengesData challenges) : base(client)
        {
            Challenges = challenges;
        }
    }
}
