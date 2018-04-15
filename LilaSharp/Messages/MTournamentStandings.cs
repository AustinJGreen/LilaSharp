using LilaSharp.Types;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LilaSharp.Messages
{
    public class MTournamentStandings : IMessage
    {
        public string Type => "tourStanding";

        [JsonProperty("d")]
        public List<PlayerStanding> Players { get; set; }
    }
}
