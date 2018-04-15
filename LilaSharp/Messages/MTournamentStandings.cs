using LilaSharp.Types;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LilaSharp.Messages
{
    public class MTournamentStandings : ITypeMessage
    {
        public string Type => "tourStanding";

        [JsonProperty("d")]
        public List<PlayerStanding> Players { get; set; }
    }
}
