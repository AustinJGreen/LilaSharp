using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MTakebackOffer : ITypeMessage, IVersionedMessage
    {
        public int Version { get; set; }

        public string Type => "takebackOffers";

        [JsonProperty("d")]
        public TakebackData Data { get; set; }
    }
}
