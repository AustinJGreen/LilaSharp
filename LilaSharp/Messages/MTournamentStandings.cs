using LilaSharp.Types;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message containing tournament standings information
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MTournamentStandings : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "tourStanding";

        /// <summary>
        /// Gets or sets the player's standings.
        /// </summary>
        /// <value>
        /// The player's standings.
        /// </value>
        [JsonProperty("d")]
        public List<PlayerStanding> Players { get; set; }
    }
}
