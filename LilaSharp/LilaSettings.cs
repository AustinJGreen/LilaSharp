namespace LilaSharp
{
    /// <summary>
    /// Class holding settings for the <see cref="LilaClient"/> class.
    /// </summary>
    public class LilaSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether reconnections should be automatic. Defaults as true.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic reconnect]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoReconnect { get; set; }

        /// <summary>
        /// Gets or sets the reconnection attempt limit. Defaults as 2.
        /// </summary>
        /// <value>
        /// The reconnection attempt limit.
        /// </value>
        public int ReconnectionAttemptLimit { get; set; }

        /// <summary>
        /// Initializes a default instance of the <see cref="LilaSettings"/> class.
        /// </summary>
        public LilaSettings() : this(true, 2)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LilaSettings"/> class.
        /// </summary>
        /// <param name="autoReconnect">if set to <c>true</c> [automatic reconnect].</param>
        /// <param name="attemptLimit">The attempt limit.</param>
        public LilaSettings(bool autoReconnect, int attemptLimit)
        {
            AutoReconnect = autoReconnect;
            ReconnectionAttemptLimit = attemptLimit;
        }

        /// <summary>
        /// A default LilaSettings instance.
        /// </summary>
        public readonly static LilaSettings Default = new LilaSettings();
    }
}
