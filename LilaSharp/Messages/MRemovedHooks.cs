using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message containing information about removed seeks/hooks
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MRemovedHooks : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "hrm";

        /// <summary>
        /// Gets or sets the concatenated ids.
        /// </summary>
        /// <value>
        /// The concatenated ids.
        /// </value>
        [JsonProperty("d")]
        public string ConcatenatedIds { get; set; }

        /// <summary>
        /// Gets the removed ids as a list.
        /// </summary>
        public string[] GetRemoved()
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
