namespace LilaSharp.Messages
{
    /// <summary>
    /// Message indicating that the forum should be reloaded
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MReloadForum : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "reload_forum";
    }
}
