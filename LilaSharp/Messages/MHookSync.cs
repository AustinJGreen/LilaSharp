using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MHookSync : IMessage
    {
        public string Type => "hli";

        [JsonProperty("d")]
        public string ConcatenatedIds { get; set; }

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
