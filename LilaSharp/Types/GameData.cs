using Newtonsoft.Json;
using System.Collections.Generic;

namespace LilaSharp.Types
{
    public class GameData
    {
        [JsonProperty("game")]
        public Board Board { get; set; }

        [JsonProperty("clock")]
        public Clock Clock { get; set; }

        [JsonProperty("player")]
        public Player Player { get; set; }

        [JsonProperty("opponent")]
        public Player Opponent { get; set; }

        [JsonProperty("oreintation")]
        public string Orientation { get; set; }

        [JsonProperty("url")]
        public Url Url { get; set; }

        [JsonProperty("pref")]
        public Pref Pref { get; set; }

        //public Tv tv { get; set; }

        [JsonProperty("steps")]
        public List<Step> Steps { get; set; }

        //public Tournament tournament { get; set; }

        public string GetID()
        {
            return Board.Id;
        }
    }
}
