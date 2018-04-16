namespace LilaSharp.Messages
{
    /// <summary>
    /// Message signaling that queued resend messages should be deleted
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MAck : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "ack";
    }
}
