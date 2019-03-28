using Newtonsoft.Json;

namespace LilaSharp.Types
{
    /// <summary>
    /// The chat object for a data of a chatroom
    /// </summary>
    public class Chat
    {
        /// <summary>
        /// Gets or sets the chat data
        /// </summary>
        [JsonProperty("data")]
        public ChatData Data { get; set; }

        /// <summary>
        /// Gets or sets the internationalization
        /// </summary>
        [JsonProperty("i8ln")]
        public I18n I18n { get; set; }

        /// <summary>
        /// Gets or sets whether this chat can be written to
        /// </summary>
        [JsonProperty("writeable")]
        public bool Writeable { get; set; }

        /// <summary>
        /// Gets or sets the note identifier
        /// </summary>
        [JsonProperty("noteId")]
        public object NoteId { get; set; }

        /// <summary>
        /// Gets or sets if this chat room is public
        /// </summary>
        [JsonProperty("public")]
        public bool IsPublic { get; set; }

        /// <summary>
        /// Gets or sets the chat permissions
        /// </summary>
        [JsonProperty("permissions")]
        public Permissions Permissions { get; set; }

        /// <summary>
        /// Gets or sets whether the chat room has been timed out for this connection
        /// </summary>
        [JsonProperty("timeout")]
        public bool Timeout { get; set; }
    }
}
