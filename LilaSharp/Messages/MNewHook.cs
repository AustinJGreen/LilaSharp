using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message containing data about new hook
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MNewHook : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "had";

        /// <summary>
        /// Gets or sets the hook.
        /// </summary>
        /// <value>
        /// The hook.
        /// </value>
        [JsonProperty("d")]
        public Hook Hook { get; set; }
    }
}
