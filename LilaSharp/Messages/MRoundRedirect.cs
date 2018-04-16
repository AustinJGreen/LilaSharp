using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message containing information about a round redirection
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    /// <seealso cref="LilaSharp.Messages.IVersionedMessage" />
    public class MRoundRedirect : ITypeMessage, IVersionedMessage
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "redirect";

        /// <summary>
        /// Gets or sets the redirect.
        /// </summary>
        /// <value>
        /// The redirect.
        /// </value>
        [JsonProperty("d")]
        public Redirect Redirect { get; set; }     
    }
}
