using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Perfs
    {
        [JsonProperty("bullet")]
        public PlayerPerf Bullet { get; set; }

        [JsonProperty("blitz")]
        public PlayerPerf Blitz { get; set; }

        [JsonProperty("classical")]
        public PlayerPerf Classical { get; set; }

        [JsonProperty("atomic")]
        public PlayerPerf Atomic { get; set; }
    }
}
