namespace LilaSharp.Messages
{
    /// <summary>
    /// Message indicating that seeks should be reloaded
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MReloadSeeks : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "reload_seeks";
    }
}
