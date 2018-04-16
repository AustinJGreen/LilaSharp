using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message containing information about a takeback offer
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    /// <seealso cref="LilaSharp.Messages.IVersionedMessage" />
    public class MTakebackOffer : ITypeMessage, IVersionedMessage
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
        public string Type => "takebackOffers";

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [JsonProperty("d")]
        public TakebackData Data { get; set; }
    }
}
