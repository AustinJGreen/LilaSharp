using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MHooks : IMessage
    {
        public string Type => "hooks";

        [JsonProperty("d")]
        public Hook[] Hooks { get; set; }
    }
}
