using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace LilaSharp.Messages
{
    public class MGameEvent : IMessage
    {
        public string Type => "b";

        [JsonProperty("d")]
        public List<JObject> Data { get; set; }
    }
}
