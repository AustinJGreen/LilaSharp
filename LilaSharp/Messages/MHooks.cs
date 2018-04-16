using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message containing information about hooks/seeks
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MHooks : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "hooks";

        /// <summary>
        /// Gets or sets the hook list.
        /// </summary>
        /// <value>
        /// The hook list.
        /// </value>
        [JsonProperty("d")]
        public Hook[] HookList { get; set; }
    }
}
