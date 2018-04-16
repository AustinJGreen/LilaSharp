using Newtonsoft.Json;

namespace LilaSharp.API
{
    /// <summary>
    /// Response for seek requests
    /// </summary>
    /// <seealso cref="LilaSharp.API.Response" />
    public class SeekResponse : Response
    {
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        [JsonProperty("hook")]
        public HookResult Result { get; set; }
    }
}
