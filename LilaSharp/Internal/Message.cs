using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NLog;
using System;
using System.Text;

namespace LilaSharp.Internal
{
    internal class Message
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private static readonly JsonSerializerSettings settings;

        /// <summary>
        /// Initializes the <see cref="Message"/> class.
        /// </summary>
        static Message()
        {
            settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                Error = OnMessageParseError
            };
        }

        /// <summary>
        /// Called when message parse error occurs.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ErrorEventArgs"/> instance containing the event data.</param>
        private static void OnMessageParseError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs e)
        {
            log.Error(e.ErrorContext.Error, "Failed to deserialize json.");
        }

        private byte[] data;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Message"/> is empty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if empty; otherwise, <c>false</c>.
        /// </value>
        public bool Empty
        {
            get
            {
                return data.Length == 0;
            }
        }

        /// <summary>
        /// Appends the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="length">The length.</param>
        public void Append(byte[] buffer, int length)
        {
            int offset = data.Length;
            Array.Resize(ref data, data.Length + length);
            Buffer.BlockCopy(buffer, 0, data, offset, length);
        }

        /// <summary>
        /// Decodes the specified encoding.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public string Decode(Encoding encoding)
        {
            return encoding.GetString(data);
        }

        /// <summary>
        /// Decodes the object.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public JObject DecodeObject(Encoding encoding)
        {
            string jsonStr = Decode(encoding);
            object obj = JsonConvert.DeserializeObject(jsonStr, settings);

            if (obj is JObject)
            {
                return (JObject)obj;
            }
            else
            {
                log.Error("Deserialized object is not a JObject. It is a {0}", obj.GetType().FullName);
                return null;
            }
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public void Delete()
        {
            data = new byte[0];
        }

        /// <summary>
        /// Finalizes this instance.
        /// </summary>
        /// <returns></returns>
        public Message Finalize()
        {
            return new Message(data);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        public Message()
        {
            data = new byte[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="array">The array.</param>
        private Message(byte[] array)
        {
            data = new byte[array.Length];
            Buffer.BlockCopy(array, 0, data, 0, array.Length);
        }
    }
}
