using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MTournamentRedirect : ITypeMessage
    {
        public string Type => "redirect";

        [JsonProperty("d")]
        public string Id { get; set; }
    }
}
