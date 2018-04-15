using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Chat
    {
        [JsonProperty("data")]
        public ChatData Data { get; set; }

        [JsonProperty("i8ln")]
        public I18n I18n { get; set; }

        [JsonProperty("writeable")]
        public bool Writeable { get; set; }

        [JsonProperty("noteId")]
        public object NoteId { get; set; }

        [JsonProperty("public")]
        public bool IsPublic { get; set; }

        [JsonProperty("permissions")]
        public Permissions Permissions { get; set; }

        [JsonProperty("timeout")]
        public bool Timeout { get; set; }
    }
}
