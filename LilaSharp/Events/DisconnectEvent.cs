namespace LilaSharp.Events
{
    /// <summary>
    /// Event for when a lichess client is disconnected
    /// </summary>
    public class DisconnectEvent : LilaEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisconnectEvent"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public DisconnectEvent(LilaClient client) : base(client)
        {

        }
    }
}
