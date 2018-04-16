using LilaSharp.Types;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message containing information about tournaments
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MTournaments : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "tournaments";

        /// <summary>
        /// Gets or sets the HTML.
        /// </summary>
        /// <value>
        /// The HTML.
        /// </value>
        [JsonProperty("d")]
        public string Html { get; set; }

        /// <summary>
        /// Gets or sets the tournament entries.
        /// </summary>
        /// <value>
        /// The tournament entries.
        /// </value>
        [JsonIgnore] //Put in by handler for more information, parsed from html.
        public List<TournamentHtmlEntry> TournamentEntries { get; set; }
    }
}
