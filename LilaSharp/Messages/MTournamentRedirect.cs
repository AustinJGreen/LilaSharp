using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MTournamentRedirect : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "redirect";

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("d")]
        public string Id { get; set; }
    }
}
