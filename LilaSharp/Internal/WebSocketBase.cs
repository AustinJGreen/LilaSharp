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
        protected object typeLock;
        protected object versionLock;
        protected List<EventTimer<Packet>> schedulers;
        protected Dictionary<string, Delegate> typeHandlers;
        protected List<TypeDelegate> versionHandlers;
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
            typeLock = new object();
            versionLock = new object();

            schedulers = new List<EventTimer<Packet>>();
            typeHandlers = new Dictionary<string, Delegate>();
            versionHandlers = new List<TypeDelegate>();

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
                System.Diagnostics.Debug.WriteLine("Cannot send null or empty text.");
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
                    System.Diagnostics.Debug.WriteLine("Received {0}", jobj.ToString());
                }

                //Lock whole statement for futureproof protection against handlers being removed
                lock (typeLock)
                {
                    if (typeHandlers.ContainsKey(type))
                    {
                        Type[] delegateArgs = typeHandlers[type].GetType().GetGenericArguments();
                        try
                        {
                            object obj = jobj.ToObject(delegateArgs[0]);
                            Task.Run(() => typeHandlers[type].DynamicInvoke(this, obj));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Error while handling \"{0}\". Check the IMessage json structure.", ex);
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Unhandled message: {0}", jobj.ToString());
                    }
                }
            }
            else if (jobj.TryGetValue("v", out JToken versionToken))
            {
                lock (versionLock)
                {
                    int parseErrors = 0;
                    for (int i = 0; i < versionHandlers.Count; i++)
                    {
                        try
                        {
                            object obj = jobj.ToObject(versionHandlers[i].Key);
                            Task.Run(() => versionHandlers[i].Value.DynamicInvoke(this, obj));
                        }
                        catch
                        {
                            parseErrors++;
                        }
                    }

                    if (parseErrors == versionHandlers.Count)
                    {
                        System.Diagnostics.Debug.WriteLine("No handlers were able to parse the IVersionedMessage.");
                    }
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
        public void AddHandler<T>(MessageHandler<T> handler) where T : ITypeMessage
        {
            ITypeMessage message = null;

            try
            {
                message = Activator.CreateInstance<T>();
            }
            catch (MissingMethodException mme)
            {
                System.Diagnostics.Debug.WriteLine(mme, "ITypeMessage type must have a zero argument constructor.");
                return;
            }

            lock (typeLock)
            {
                typeHandlers.Add(message.Type, handler);
            }
        }

        /// <summary>
        /// Adds a delegate to handle messages with version information but no type
        /// </summary>
        /// <typeparam name="T">The version type event</typeparam>
        /// <param name="handler">The handler.</param>
        public void AddVersionHandler<T>(MessageHandler<T> handler) where T : IVersionedMessage
        {
            TypeDelegate td = new TypeDelegate()
            {
                Key = typeof(T),
                Value = handler
            };

            lock (versionLock)
            {
                versionHandlers.Add(td);
            }
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
                EventTimer<Packet> timer = new EventTimer<Packet>(OnPacketScheduled, period, packet)
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
        private void OnJsonParseError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.ErrorContext.Error, "Failed to deserialize json.");
        }
    }
}
