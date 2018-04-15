namespace LilaSharp.Events
{
    public class LatencyEvent : LilaEvent
    {
        /// <summary>
        /// Gets or sets the latency.
        /// </summary>
        /// <value>
        /// The latency.
        /// </value>
        public int Latency { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LatencyEvent"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="latency">The latency.</param>
        public LatencyEvent(LilaClient client, int latency) : base(client)
        {
            Latency = latency;
        }
    }
}
