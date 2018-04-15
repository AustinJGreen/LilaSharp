using Newtonsoft.Json;
using System.Collections.Generic;

namespace LilaSharp.Types
{
    public class PageStandings
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("players")]
        public List<TournamentPlayer> Players { get; set; }
    }
}
