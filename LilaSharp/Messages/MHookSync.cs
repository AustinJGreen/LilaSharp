using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message containing information about new list of synchronized hooks/seeks
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MHookSync : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "hli";

        /// <summary>
        /// Gets or sets the concatenated list of hook ids.
        /// </summary>
        /// <value>
        /// The concatenated list of hook ids.
        /// </value>
        [JsonProperty("d")]
        public string ConcatenatedIds { get; set; }

        /// <summary>
        /// Gets the sychronized list of hooks as a list
        /// </summary>
        public string[] GetSynchronized()
        {
            int amount = ConcatenatedIds.Length / 8;
            string[] ids = new string[amount];

            for (int i = 0; i < amount; i++)
            {
                ids[i] = ConcatenatedIds.Substring(i * 8, 8);
            }

            return ids;
        }
    }
}
