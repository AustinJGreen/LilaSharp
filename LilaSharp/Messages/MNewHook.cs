using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MNewHook : ITypeMessage
    {
        public string Type => "had";

        [JsonProperty("d")]
        public Hook Hook { get; set; }
    }
}
