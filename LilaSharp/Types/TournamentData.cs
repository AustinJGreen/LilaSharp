using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LilaSharp.Types
{
    public class TournamentData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("system")]
        public string System { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("perf")]
        public Perf Perf { get; set; }

        [JsonProperty("nbPlayers")]
        public int PlayerCount { get; set; }

        [JsonProperty("minutes")]
        public int Minutes { get; set; }

        [JsonProperty("clock")]
        public Clock Clock { get; set; }

        [JsonProperty("verdicts")]
        public Verdicts Verdicts { get; set; }

        [JsonProperty("variant")]
        public string Variant { get; set; }

        [JsonProperty("isStarted")]
        public bool IsStarted { get; set; }

        [JsonProperty("isFinished")]
        public bool IsFinished { get; set; }

        [JsonProperty("startsAt")]
        public DateTime StartsAt { get; set; }

        [JsonProperty("duels")]
        public List<object> Duels { get; set; }

        [JsonProperty("standing")]
        public PageStandings Standing { get; set; }

        [JsonProperty("socketVersion")]
        public int SocketVersion { get; set; }

        [JsonProperty("greatPlayer")]
        public PlayerData GreatPlayer { get; set; }

        [JsonProperty("secondsToFinish")]
        public int SecondsToFinish { get; set; }

        [JsonProperty("me")]
        public TournamentPlayerStatus Me { get; set; }

        [JsonProperty("featured")]
        public Featured Featured { get; set; }
    }
}
