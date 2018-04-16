using Newtonsoft.Json;

namespace LilaSharp.API
{
    public class Response
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Response"/> is ok.
        /// </summary>
        /// <value>
        ///   <c>true</c> if ok; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("ok")]
        public bool Ok { get; set; }
    }
}
