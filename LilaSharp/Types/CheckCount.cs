using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class CheckCount
    {
        [JsonProperty("white")]
        public int ChecksOnWhite { get; set; }

        [JsonProperty("black")]
        public int ChecksOnBlack { get; set; }
    }
}
