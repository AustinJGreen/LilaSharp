using LilaSharp.Internal;
using LilaSharp.Messages;
using LilaSharp.Packets;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Threading.Tasks;

namespace LilaSharp
{
    /// <summary>
    /// Class for managing a socket connection to lichess. To connect to lichess use <see cref="LilaClient"/>.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public sealed class LilaSocket : IDisposable
    {
        private WebSocketBase socket;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LilaSocket"/> is debug.
        /// </summary>
        /// <value>
        ///   <c>true</c> if debug; otherwise, <c>false</c>.
        /// </value>
        internal bool Debug { get { return socket.Debug; } set { socket.Debug = value; } }

        /// <summary>
        /// Occurs on disconnection.
        /// </summary>
        internal event EventHandler<SocketDisconnectArgs> OnDisconnect
        {
            add
            {
                socket.OnDisconnect = value;
            }
            remove
            {
                socket.OnDisconnect = null;
            }
        }

        /// <summary>
        /// Determines whether this instance is connected.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </returns>
        public bool IsConnected()
        {
            return socket.IsConnected();
        }

        /// <summary>
        /// Connects the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public bool Connect(string url)
        {
            return socket.Connect(url);
        }

        /// <summary>
        /// Connects the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public bool Connect(Uri uri)
        {
            return socket.Connect(uri);
        }

        /// <summary>
        /// Connects the asynchronous.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public Task ConnectAsync(string url)
        {
            return socket.ConnectAsync(url).Task;
        }

        /// <summary>
        /// Connects the asynchronous.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public Task ConnectAsync(Uri uri)
        {
            return socket.ConnectAsync(uri).Task;
        }

        /// <summary>
        /// Gets the URI.
        /// </summary>
        /// <returns></returns>
        public Uri GetUri()
        {
            if (socket != null)
            {
                return socket.GetUri();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void Send(string data)
        {
            socket.Send(data);
        }

        /// <summary>
        /// Sends the specified packet.
        /// </summary>
        /// <param name="packet">The packet.</param>
        public void Send(Packet packet)
        {
            socket.Send(packet);
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void Send(byte[] data)
        {
            socket.Send(data);
        }

        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        public void Disconnect()
        {
            socket.Disconnect();
        }

        /// <summary>
        /// Disconnects asynchronously.
        /// </summary>
        /// <returns></returns>
        public Task DisconnectAsync()
        {
            return socket.DisconnectAsync().Task;
        }

        /// <summary>
        /// Reconnects this instance.
        /// </summary>
        public void Reconnect()
        {
            socket.Reconnect();
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            socket.Reset();
        }

        /// <summary>
        /// Adds the handler.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler">The handler.</param>
        internal void AddHandler<T>(MessageHandler<T> handler) where T : ITypeMessage
        {
            socket.AddHandler(handler);
        }

        /// <summary>
        /// Adds a delegate to handle messages with version information but no type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler">The handler.</param>
        internal void AddVersionHandler<T>(MessageHandler<T> handler) where T : IVersionedMessage
        {
            socket.AddVersionHandler(handler);
        }

        /// <summary>
        /// Invokes the message.
        /// </summary>
        /// <param name="jobj">The jobj.</param>
        internal void InvokeMessage(JObject jobj)
        {
            socket.InvokeMessage(jobj);
        }

        /// <summary>
        /// Schedules the packet.
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="period">The period.</param>
        public void SchedulePacket(Packet packet, int period)
        {
            socket.SchedulePacket(packet, period);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            socket.Dispose();
        }

        /// <summary>
        /// Adds the cookies.
        /// </summary>
        /// <param name="cookie">The cookie.</param>
        public void AddCookies(Cookie cookie)
        {
            socket.Cookies.Add(cookie);
        }

        /// <summary>
        /// Adds the cookies.
        /// </summary>
        /// <param name="cc">The cc.</param>
        public void AddCookies(CookieCollection cc)
        {
            socket.Cookies.Add(cc);
        }

        /// <summary>
        /// Gets the cookies.
        /// </summary>
        /// <returns></returns>
        public CookieCollection GetCookies()
        {
            return socket.Cookies;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LilaSocket"/> class.
        /// </summary>
        /// <param name="name">The name or identifier of the socket.</param>
        /// <param name="impl">The underlying implementation to use.</param>
        internal LilaSocket(string name, ResourceType impl)
        {
            switch (impl)
            {
                case ResourceType.Task:
                    socket = new TaskSocket(name);
                    break;
                case ResourceType.Thread:
                    socket = new ThreadSocket(name);
                    break;
            }

            socket.Cookies = new CookieCollection();
        }
    }
}
