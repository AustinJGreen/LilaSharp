using Newtonsoft.Json;

namespace LilaSharp.Api
{
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
