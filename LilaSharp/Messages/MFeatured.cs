using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MFeatured : ITypeMessage
    {
        public string Type => "featured";

        [JsonProperty("d")]
        public FeaturedData Data { get; set; }
    }
}
