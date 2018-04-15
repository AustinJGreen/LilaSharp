using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MHooks : ITypeMessage
    {
        public string Type => "hooks";

        [JsonProperty("d")]
        public Hook[] HookList { get; set; }
    }
}
