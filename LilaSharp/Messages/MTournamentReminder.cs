using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    public class MTournamentReminder : ITypeMessage
    {
        public string Type => "tournamentReminder";

        [JsonProperty("d")]
        public ReminderData Data { get; set; }
    }
}
