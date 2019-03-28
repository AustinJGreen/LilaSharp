using LilaSharp.Types;

namespace LilaSharp.Events
{
    /// <summary>
    /// Event for when a move is received
    /// </summary>
    public class LilaGameMoveEvent : LilaGameEvent
    {
        /// <summary>
        /// Gets the move.
        /// </summary>
        /// <value>
        /// The move.
        /// </value>
        public MoveData Move { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LilaGameMoveEvent"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="game">The game.</param>
        /// <param name="move">The move.</param>
        public LilaGameMoveEvent(LilaClient client, LilaGame game, MoveData move) : base(client, game)
        {
            Move = move;
        }
    }
}
