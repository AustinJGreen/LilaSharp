using Newtonsoft.Json;

namespace LilaSharp.API
{
    /// <summary>
    /// Result from creating hook
    /// </summary>
    public class HookResult
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
