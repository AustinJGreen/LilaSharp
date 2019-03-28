namespace LilaSharp.Events
{
    /// <summary>
    /// Event for when client conencts to lichess
    /// </summary>
    public class ConnectEvent : LilaEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectEvent"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public ConnectEvent(LilaClient client) : base(client)
        {

        }
    }
}
