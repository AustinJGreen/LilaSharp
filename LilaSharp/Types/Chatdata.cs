using Newtonsoft.Json;
using System.Collections.Generic;

namespace LilaSharp.Types
{
    /// <summary>
    /// Chat data object
    /// </summary>
    public class ChatData
    {
        /// <summary>
        /// Gets or sets the chat data identifier
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the sender
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the lines sent
        /// </summary>
        [JsonProperty("lines")]
        public List<object> Lines { get; set; }

        /// <summary>
        /// Gets or sets the sender userid
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets whether a login is required
        /// </summary>
        [JsonProperty("loginRequired")]
        public bool LoginRequired { get; set; }

        /// <summary>
        /// Gets or sets if this connection is restricted to chat
        /// </summary>
        [JsonProperty("restricted")]
        public bool Restricted { get; set; }
    }
}
