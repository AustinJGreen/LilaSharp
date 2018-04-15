using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Promotion
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("pieceClass")]
        public string PieceClass { get; set; }
    }
}
