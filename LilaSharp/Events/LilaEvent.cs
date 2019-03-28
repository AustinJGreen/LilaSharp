using System;

namespace LilaSharp.Events
{
    /// <summary>
    /// Generic event for all lichess client event
    /// </summary>
    public class LilaEvent : EventArgs
    {
        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public LilaClient Client { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LilaEvent"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public LilaEvent(LilaClient client)
        {
            Client = client;
        }
    }
}
