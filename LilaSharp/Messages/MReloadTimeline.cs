namespace LilaSharp.Messages
{
    /// <summary>
    /// Message indicating that the timeline should be reloaded
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MReloadTimeline : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "reload_timeline";
    }
}
