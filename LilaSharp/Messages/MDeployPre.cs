namespace LilaSharp.Messages
{
    /// <summary>
    /// Message indicating that lichess will restart soon
    /// </summary>
    public class MDeployPre : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "deployPre";
    }
}
