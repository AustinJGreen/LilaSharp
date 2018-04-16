namespace LilaSharp.Messages
{
    /// <summary>
    /// Message containing version information
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.IVersionedMessage" />
    public class MVersion : IVersionedMessage
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }
    }
}
