using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MTournamentRedirect : IMessage
    {
        public string Type => "redirect";

        [JsonProperty("d")]
        public string Id { get; set; }
    }
}
