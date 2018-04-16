using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message containing multiple game events
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MGameMessages : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "b";

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [JsonProperty("d")]
        public List<JObject> Data { get; set; }
    }
}
