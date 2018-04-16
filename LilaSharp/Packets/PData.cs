using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilaSharp.Packets
{
    /// <summary>
    /// Packet for data with wrapped packets
    /// </summary>
    /// <typeparam name="P">The type of packet being wrapped</typeparam>
    /// <seealso cref="LilaSharp.Packets.Packet" />
    public class PData<P> : Packet where P : Packet
    {
        private P data;

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public override string Type
        {
            get
            {
                return data.Type;
            }
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [JsonProperty("d")]
        public P Data { get { return data; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="PData{P}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public PData(P data) : base()
        {
            this.data = (P)data.DataClone();
        }
    }
}
