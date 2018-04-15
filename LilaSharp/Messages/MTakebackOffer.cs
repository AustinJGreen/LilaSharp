using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MTakebackOffer : IGameMessage
    {
        public int Version { get; set; }

        public string Type => "takebackOffers";

        [JsonProperty("d")]
        public TakebackData Data { get; set; }
    }
}
