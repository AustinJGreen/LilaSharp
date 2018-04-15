using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MRoundRedirect : ITypeMessage, IVersionedMessage
    {
        public int Version { get; set; }

        public string Type => "redirect";

        [JsonProperty("d")]
        public Redirect Redirect { get; set; }     
    }
}
