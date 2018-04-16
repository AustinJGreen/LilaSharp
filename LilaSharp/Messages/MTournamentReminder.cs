using LilaSharp.Types;
using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    /// <summary>
    /// Message containing information about tournament reminder
    /// </summary>
    /// <seealso cref="LilaSharp.Messages.ITypeMessage" />
    public class MTournamentReminder : ITypeMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type => "tournamentReminder";

        /// <summary>
        /// Gets or sets the reminder data.
        /// </summary>
        /// <value>
        /// The reminder data.
        /// </value>
        [JsonProperty("d")]
        public ReminderData Data { get; set; }
    }
}
