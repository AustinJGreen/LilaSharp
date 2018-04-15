using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Step
    {
        [JsonProperty("ply")]
        public int Ply { get; set; }

        [JsonProperty("uci")]
        public string Uci { get; set; }

        [JsonProperty("san")]
        public string San { get; set; }

        [JsonProperty("fen")]
        public string Fen { get; set; }
    }
}
