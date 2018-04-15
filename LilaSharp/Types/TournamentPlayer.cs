using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class TournamentPlayer
    {
        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rating")]
        public int Rating { get; set; }

        [JsonProperty("ratingDiff")]
        public int RatingDiff { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("sheet")]
        public Sheet Sheet { get; set; }

        [JsonProperty("withdraw")]
        public bool? Withdraw { get; set; }

        [JsonProperty("provisional")]
        public bool? Provisional { get; set; }
    }
}
