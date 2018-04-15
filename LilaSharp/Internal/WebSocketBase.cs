using LilaSharp.Messages;
using LilaSharp.Packets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LilaSharp.Internal
{
    internal abstract class WebSocketBase : WebSocketInternal
    {
        protected List<LilaTimer<Packet>> schedulers;
        protected Dictionary<string, Delegate> handlers;
        protected JsonSerializerSettings jsonSettings;

        /// <summary>
        /// The OnDisconnect delegate
        /// </summary>
        public EventHandler<SocketDisconnectArgs> OnDisconnect;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketBase"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        protected WebSocketBase(string name) : base(name)
        {
            schedulers = new List<LilaTimer<Packet>>();
            handlers = new Dictionary<string, Delegate>();

            jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                Error = OnJsonParseError,
                ContractResolver = new PacketResolver()
            };
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="finalize"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool finalize)
        {
            if (schedulers != null)
            {
                for (int i = 0; i < schedulers.Count; i++)
                {
                    schedulers[i].Dispose();
                    schedulers[i] = null;
                }
            }

            schedulers = null;
            
            if (finalize)
            {
                GC.SuppressFinalize(this);
            }

            base.Dispose(finalize);
        }

        /// <summary>
        /// Sends the specified text synchronously.
        /// </summary>
        /// <param name="text">The text.</param>
        public void Send(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                Send(Encoding.GetBytes(text));
            }
            else
            {
                log.Warn("Cannot send null or empty text.");
            }
        }

        /// <summary>
        /// Sends the specified packet sychronously.
        /// </summary>
        /// <param name="packet">The packet.</param>
        public void Send(Packet packet)
        {
            packet.LastSent = DateTime.Now;
            string serialized = JsonConvert.SerializeObject(packet, jsonSettings);
            Send(serialized);
        }

        /// <summary>
        /// Invokes the handler for a json object.
        /// </summary>
        /// <param name="jobj">The jobj.</param>
        internal void InvokeMessage(JObject jobj)
        {
            if (jobj.TryGetValue("t", out JToken typeToken))
            {
                JValue tokenValue = (JValue)typeToken;
                string type = tokenValue.Value<string>();

                if (Debug)
                {
                    log.ConditionalDebug("Received {0}", jobj.ToString());
                }

                if (handlers.ContainsKey(type))
                {
                    Type[] delegateArgs = handlers[type].GetType().GetGenericArguments();

                    try
                    {
                        object obj = jobj.ToObject(delegateArgs[0]);
                        Task handlingTask = Task.Run(() => handlers[type].DynamicInvoke(this, obj));
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex, "Error while handling \"{0}\". Check the IMessage json structure.", type);
                    }
                }
                else
                {
                    log.Warn("Unhandled message: {0}", jobj.ToString());
                }
            }
        }

        /// <summary>
        /// Handles the message.
        /// </summary>
        /// <param name="m">The message.</param>
        protected void HandleMessage(Message m)
        {
            if (m != null)
            {
                //Try and parse into json
                JObject jobj = m.DecodeObject(Encoding);
                InvokeMessage(jobj);
            }
        }

        /// <summary>
        /// Adds a handler for handling a lichess message type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler">The handler.</param>
        public void AddHandler<T>(MessageHandler<T> handler) where T : IMessage
        {
            IMessage message = null;

            try
            {
                message = Activator.CreateInstance<T>();
            }
            catch (MissingMethodException mme)
            {
                log.Error(mme, "IMessage type must have a zero argument constructor.");
                return;
            }

            handlers.Add(message.Type, handler);
        }

        /// <summary>
        /// Stops the schedulers.
        /// </summary>
        protected void StopSchedulers()
        {
            if (schedulers != null)
            {
                for (int i = 0; i < schedulers.Count; i++)
                {
                    schedulers[i].Stop();
                }
            }
        }

        /// <summary>
        /// Starts the schedulers.
        /// </summary>
        protected void StartSchedulers()
        {
            if (schedulers != null)
            {
                for (int i = 0; i < schedulers.Count; i++)
                {
                    schedulers[i].Start();
                }
            }
        }

        /// <summary>
        /// Schedules a packet.
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="period">The period between firings.</param>
        public void SchedulePacket(Packet packet, int period)
        {
            if (schedulers != null)
            {
                LilaTimer<Packet> timer = new LilaTimer<Packet>(OnPacketScheduled, period, packet)
                {
                    Debug = Debug
                };

                schedulers.Add(timer);

                if (IsConnected())
                {
                    timer.Start();
                }
            }
        }

        /// <summary>
        /// Called when packet scheduled.
        /// </summary>
        /// <param name="packet">The packet.</param>
        private void OnPacketScheduled(Packet packet)
        {
            if (IsConnected())
            {
                Send(packet);
            }
            else
            {
                StopSchedulers();
            }
        }

        /// <summary>
        /// Called when connected.
        /// </summary>
        public override void OnConnect()
        {
            StartListening();
            if (schedulers != null)
            {
                for (int i = 0; i < schedulers.Count; i++)
                {
                    schedulers[i].Start();
                }
            }
        }

        /// <summary>
        /// Called when json parse error ocurrs.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ErrorEventArgs"/> instance containing the event data.</param>
        private void OnJsonParseError(object sender, ErrorEventArgs e)
        {
            log.Error(e.ErrorContext.Error, "Failed to deserialize json.");
        }
    }
}
