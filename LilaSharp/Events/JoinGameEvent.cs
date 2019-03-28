namespace LilaSharp.Events
{
    /// <summary>
    /// Event for when a game is joined
    /// </summary>
    public class JoinGameEvent : LilaEvent
    {
        /// <summary>
        /// Gets or sets the game.
        /// </summary>
        /// <value>
        /// The game.
        /// </value>
        public LilaGame Game { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinGameEvent"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="game">The game.</param>
        public JoinGameEvent(LilaClient client, LilaGame game) : base(client)
        {
            Game = game;
        }
    }
}
