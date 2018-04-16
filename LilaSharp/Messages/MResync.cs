namespace LilaSharp.Messages
{
    /// <summary>
    /// Message indicating that you are out of sync and that you should reload
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MResync : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "resync";
    }
}
