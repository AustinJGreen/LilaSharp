using LilaSharp.Types;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LilaSharp.Messages
{
    public class MTournaments : IMessage
    {
        public string Type => "tournaments";

        [JsonProperty("d")]
        public string Html { get; set; }

        [JsonIgnore] //Put in by handler for more information, parsed from html
        public List<TournamentHtmlEntry> Tournaments { get; set; }
    }
}
