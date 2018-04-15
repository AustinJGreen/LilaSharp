using System;

namespace LilaSharp.Internal
{
    internal class SocketDisconnectArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SocketDisconnectArgs"/> is initiated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if initiated; otherwise, <c>false</c>.
        /// </value>
        public bool Initiated { get; set; }

        /// <summary>
        /// Gets or sets the reconnection attempts.
        /// </summary>
        /// <value>
        /// The reconnection attempts.
        /// </value>
        public int ReconnectionAttempts { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketDisconnectArgs"/> class.
        /// </summary>
        /// <param name="initiated">if set to <c>true</c> [initiated].</param>
        /// <param name="reconnectionAttempts">The reconnection attempts.</param>
        public SocketDisconnectArgs(bool initiated, int reconnectionAttempts)
        {
            Initiated = initiated;
            ReconnectionAttempts = reconnectionAttempts;
        }
    }
}
