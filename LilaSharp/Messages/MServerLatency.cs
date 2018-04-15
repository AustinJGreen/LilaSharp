using Newtonsoft.Json;

namespace LilaSharp.Messages
{
    class MServerLatency : ITypeMessage
    {
        public string Type => "mlat";

        [JsonProperty("d")]
        public int Latency { get; set; }
    }
}
