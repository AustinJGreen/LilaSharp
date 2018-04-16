using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Interface for messages containing a type
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.IMessage" />
    public interface ITypeMessage : IMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [JsonProperty("t")]
        string Type { get; }
    }
}
