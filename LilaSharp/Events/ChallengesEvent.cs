using LilaSharp.Types;

namespace LilaSharp.Events
{
    public class ChallengesEvent : LilaEvent
    {
        /// <summary>
        /// Gets or sets the challenges.
        /// </summary>
        /// <value>
        /// The challenges.
        /// </value>
        public Challenges Challenges { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChallengesEvent"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="challenges">The challenges.</param>
        public ChallengesEvent(LilaClient client, Challenges challenges) : base(client)
        {
            Challenges = challenges;
        }
    }
}
