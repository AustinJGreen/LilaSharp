using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    class MServerLatency : IMessage
    {
        public string Type => "mlat";

        [JsonProperty("d")]
        public int Latency { get; set; }
    }
}
