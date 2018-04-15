using Newtonsoft.Json;
using System.Collections.Generic;

namespace LilaSharp.Types
{
    public class MoveData
    {
        [JsonProperty("uci")]
        public string Uci { get; set; }

        [JsonProperty("san")]
        public string San { get; set; }

        [JsonProperty("fen")]
        public string Fen { get; set; }

        [JsonProperty("ply")]
        public int Ply { get; set; }

        [JsonProperty("promotion")]
        public Promotion Promotion { get; set; }

        [JsonProperty("dests")]
        public Dictionary<string, string> Legal { get; set; }

        [JsonProperty("clock")]
        public ClockData Clock { get; set; }

        public List<LilaMove> GetMoves()
        {
            List<LilaMove> moves = new List<LilaMove>();
            if (Legal == null)
            {
                return moves;
            }

            foreach(KeyValuePair<string, string> legalPair in Legal)
            {
                string from = legalPair.Key;

                string targets = legalPair.Value;
                for (int i = 0; i < targets.Length; i += 2)
                {
                    string target = targets.Substring(i, 2);
                    moves.Add(new LilaMove(from, target, null));
                }
            }

            return moves;
        }
    }
}
