using Newtonsoft.Json;
using System;

namespace LilaSharp.Packets
{
    /// <summary>
    /// Class containing information about packets being sent.
    /// </summary>
    public class Packet
    {
        private string type;

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [JsonProperty("t")]
        public virtual string Type { get { return type; } }

        /// <summary>
        /// Gets or sets the time the packet was last sent.
        /// </summary>
        /// <value>
        /// The time the packet was last sent.
        /// </value>
        [JsonIgnore]
        public DateTime LastSent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the type should be serialized
        /// </summary>
        /// <value>
        ///   <c>true</c> if [should serialize type]; otherwise, <c>false</c>.
        /// </value>
        [JsonIgnore]
        public bool ShouldSerializeType { get; set; }

        /// <summary>
        /// Clones the packets data
        /// </summary>
        public Packet DataClone()
        {
            Packet p = (Packet)MemberwiseClone();
            p.ShouldSerializeType = false;
            return p;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Packet"/> class.
        /// </summary>
        public Packet()
        {
            ShouldSerializeType = true;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Packet"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public Packet(string type) : this()
        {
            this.type = type;
        }
    }
}
