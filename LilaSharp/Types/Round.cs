using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Round
    {
        [JsonProperty("data")]
        public GameData Game { get; set; }

        [JsonProperty("writeable")]
        public bool Writeable { get; set; }

        [JsonProperty("noteId")]
        public string NoteId { get; set; }

        [JsonProperty("public")]
        public bool IsPublic { get; set; }

        [JsonProperty("permissions")]
        public Permissions Permissions { get; set; }

        [JsonProperty("timeout")]
        public bool Timeout { get; set; }
    }
}
