using Newtonsoft.Json;

namespace LilaSharp.Types
{
    public class Pref
    {
        [JsonProperty("animationDuration")]
        public int AnimationDuration { get; set; }

        [JsonProperty("coords")]
        public int Coords { get; set; }

        [JsonProperty("replay")]
        public int Replay { get; set; }

        [JsonProperty("autoQueen")]
        public int AutoQueen { get; set; }

        [JsonProperty("clockTenths")]
        public int ClockTenths { get; set; }

        [JsonProperty("moveEvent")]
        public int MoveEvent { get; set; }

        [JsonProperty("clockBar")]
        public bool ClockBar { get; set; }

        [JsonProperty("clockSound")]
        public bool ClockSound { get; set; }

        [JsonProperty("confirmResign")]
        public bool ConfirmResign { get; set; }

        [JsonProperty("rookCastle")]
        public bool RookCastle { get; set; }

        [JsonProperty("highlight")]
        public bool Highlight { get; set; }

        [JsonProperty("destination")]
        public bool Destination { get; set; }

        [JsonProperty("enablePremove")]
        public bool EnablePremove { get; set; }

        [JsonProperty("showCaptured")]
        public bool ShowCaptured { get; set; }
    }
}
