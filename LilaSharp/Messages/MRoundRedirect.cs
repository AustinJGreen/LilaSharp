using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MRoundRedirect : IGameMessage
    {
        public int Version { get; set; }

        public string Type => "redirect";

        [JsonProperty("d")]
        public Redirect Redirect { get; set; }     
    }
}
