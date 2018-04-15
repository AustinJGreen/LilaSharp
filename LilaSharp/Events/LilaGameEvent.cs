namespace LilaSharp.Events
{
    public class LilaGameEvent : LilaEvent
    {
        /// <summary>
        /// Gets the game.
        /// </summary>
        /// <value>
        /// The game.
        /// </value>
        public LilaGame Game { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LilaGameEvent"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="game">The game.</param>
        public LilaGameEvent(LilaClient client, LilaGame game) : base(client)
        {
            Game = game;
        }
    }
}
