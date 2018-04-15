using LilaSharp.Packets;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LilaSharp.Internal
{
    /// <summary>
    /// Underlying internal socket implementation utilizing <see cref="System.Net.WebSockets.ClientWebSocket"/>.
    /// </summary>
    /// <seealso cref="LilaSharp.LilaDebug" />
    /// <seealso cref="System.IDisposable" />
    internal abstract class WebSocketInternal : LilaDebug, IDisposable
    {
        protected Logger log;

        protected bool disconnectInitiated = false;
        protected int reconnectionAttempts = 0;
        protected Uri socketUri;

        private object socketSendLock;

        protected ClientWebSocket socket;
        protected CancellationTokenSource recvToken;

        protected Queue<byte[]> sendQueue;

        private CookieCollection _cookies;
        public CookieCollection Cookies
        {
            get
            {
                return _cookies;
            }
            set
            {
                _cookies = value;
            }
        }

        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        /// <value>
        /// The encoding.
        /// </value>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Gets or sets the connect timeout.
        /// </summary>
        /// <value>
        /// The connect timeout.
        /// </value>
        public int ConnectTimeout { get; set; }

        /// <summary>
        /// Gets or sets the disconnect timeout.
        /// </summary>
        /// <value>
        /// The disconnect timeout.
        /// </value>
        public int DisconnectTimeout { get; set; }

        /// <summary>
        /// Gets or sets the receive timeout.
        /// </summary>
        /// <value>
        /// The receive timeout.
        /// </value>
        public int ReceiveTimeout { get; set; }

        /// <summary>
        /// Gets or sets the send timeout.
        /// </summary>
        /// <value>
        /// The send timeout.
        /// </value>
        public int SendTimeout { get; set; }

        protected int _recvBufferSize;

        /// <summary>
        /// Gets or sets the size of the receive buffer.
        /// </summary>
        /// <value>
        /// The size of the receive buffer.
        /// </value>
        /// <exception cref="InvalidOperationException">Cannot change the buffer of a disposed WebSocket.</exception>
        /// <exception cref="ArgumentException">ReceiveBufferSize must be greater than or equal to 256.</exception>
        public int ReceiveBufferSize
        {
            get
            {
                return _recvBufferSize;
            }
            set
            {
                if (socket == null)
                {
                    throw new InvalidOperationException("Cannot change the buffer of a disposed WebSocket.");
                }

                if (value < 256)
                {
                    throw new ArgumentException("ReceiveBufferSize must be greater than or equal to 256.");
                }

                _recvBufferSize = value;
                socket.Options.SetBuffer(_recvBufferSize, _sendBufferSize);
            }
        }

        protected int _sendBufferSize;

        /// <summary>
        /// Gets or sets the size of the send buffer.
        /// </summary>
        /// <value>
        /// The size of the send buffer.
        /// </value>
        /// <exception cref="InvalidOperationException">Cannot change the buffer of a disposed WebSocket.</exception>
        /// <exception cref="ArgumentException">SendBufferSize must be greater than or equal to 16. - value</exception>
        public int SendBufferSize
        {
            get
            {
                return _sendBufferSize;
            }
            set
            {
                if (socket == null)
                {
                    throw new InvalidOperationException("Cannot change the buffer of a disposed WebSocket.");
                }

                if (value < 16)
                {
                    throw new ArgumentException("SendBufferSize must be greater than or equal to 16.", "value");
                }

                _sendBufferSize = value;
                socket.Options.SetBuffer(_recvBufferSize, _sendBufferSize);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketInternal"/> class.
        /// </summary>
        /// <param name="name">The name of the implementation.</param>
        internal WebSocketInternal(string name)
        {
            log = LogManager.GetLogger(name);

            Encoding = Encoding.UTF8;

            socket = new ClientWebSocket();
            socket.Options.Cookies = new CookieContainer();

            sendQueue = new Queue<byte[]>();

            socketSendLock = new object();

            ConnectTimeout = 5000;
            DisconnectTimeout = 2000;
            ReceiveTimeout = 2500;
            SendTimeout = 2500;

            _recvBufferSize = 128;
            _sendBufferSize = 128;
        }

        /// <summary>
        /// Aborts this socket.
        /// </summary>
        public void Abort()
        {
            if (socket != null)
            {
                disconnectInitiated = true;
                socket.Abort();
            }
        }

        /// <summary>
        /// Called when close output callsback.
        /// </summary>
        /// <param name="obj">The task calling back.</param>
        protected void OnCloseOutput(Task obj)
        {
            //Reset socket when socket closes
            Reset();
        }

        /// <summary>
        /// Closes the socket internally.
        /// </summary>
        protected void CloseInternal()
        {
            if (socket != null)
            {
                switch (socket.State)
                {
                    case WebSocketState.Connecting:
                        if (!SpinWait.SpinUntil(NotConnecting, ConnectTimeout))
                        {
                            Abort();
                        }
                        break;
                    case WebSocketState.Open:
                        Disconnect();
                        if (!SpinWait.SpinUntil(IsClosed, DisconnectTimeout))
                        {
                            Abort();
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Resets this socket.
        /// </summary>
        public void Reset()
        {
            socket = new ClientWebSocket();
            socket.Options.Cookies = new CookieContainer();
            sendQueue.Clear();
        }

        /// <summary>
        /// Reconnects the socket to the last endpoint.
        /// </summary>
        public void Reconnect()
        {
            Reset();
            int before = reconnectionAttempts;
            //Connect resets reconnection
            //This connection is automatic and a reconnection so don't reset counter
            Connect(socketUri);
            reconnectionAttempts = before + 1;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="finalize"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool finalize)
        {
            log.ConditionalTrace("~WebSocketInternal");
            Disconnect();
            CloseInternal();
            StopListening();
            if (socket != null)
            {
                socket.Dispose();
                socket = null;
            }

            if (finalize)
            {
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Gets the connected URI.
        /// </summary>
        /// <returns></returns>
        public Uri GetUri()
        {
            return socketUri;
        }

        /// <summary>
        /// Starts listening.
        /// </summary>
        protected abstract void StartListening();

        /// <summary>
        /// Stops listening.
        /// </summary>
        protected abstract void StopListening();

        /// <summary>
        /// Connects to the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Invalid url. - url</exception>
        public bool Connect(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                throw new ArgumentException("Invalid url.", "url");
            }

            return Connect(uri);
        }

        /// <summary>
        /// Connects to the specified URI asynchronously.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Invalid url. - url</exception>
        public WebSocketTask ConnectAsync(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                throw new ArgumentException("Invalid url.", "url");
            }

            return ConnectAsync(uri);
        }

        /// <summary>
        /// Connects to the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public bool Connect(Uri uri)
        {
            WebSocketTask task = ConnectAsync(socketUri = uri);
            task.Wait(ConnectTimeout);
            return task.IsSuccess();
        }

        /// <summary>
        /// Connects to the specified URI asynchronously.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Cannot connect using a disposed socket.</exception>
        public WebSocketTask ConnectAsync(Uri uri)
        {
            if (socket == null)
            {
                throw new InvalidOperationException("Cannot connect using a disposed socket.");
            }

            if (Cookies != null)
            {
                socket.Options.Cookies.Add(Cookies);
            }

            if (socket.State != WebSocketState.Connecting && !IsConnected())
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                Task task = socket.ConnectAsync(uri, tokenSource.Token);
                task.ContinueWith(OnConnectEnd);
                return new WebSocketTask(task, tokenSource);
            }
            else
            {
                return new WebSocketTask();
            }
        }

        /// <summary>
        /// Called when connected.
        /// </summary>
        public abstract void OnConnect();

        /// <summary>
        /// Called when connect callsback.
        /// </summary>
        /// <param name="connectTask">The connect task.</param>
        protected void OnConnectEnd(Task connectTask)
        {
            if (IsConnected())
            {
                OnConnect();

                while (sendQueue.Count > 0)
                {
                    Send(sendQueue.Dequeue());
                }
            }
        }

        /// <summary>
        /// Disconnects this socket.
        /// </summary>
        public void Disconnect()
        {
            WebSocketTask task = DisconnectAsync();
            task.Wait(DisconnectTimeout);
        }

        /// <summary>
        /// Disconnects the socket asynchronously.
        /// </summary>
        public WebSocketTask DisconnectAsync()
        {
            return DisconnectAsync(WebSocketCloseStatus.NormalClosure, string.Empty);
        }

        /// <summary>
        /// Disconnects the socket asynchronously.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="statusDescription">The status description.</param>
        /// <returns></returns>
        protected WebSocketTask DisconnectAsync(WebSocketCloseStatus status, string statusDescription)
        {
            if (socket != null && IsConnected())
            {
                disconnectInitiated = true;
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                Task task = socket.CloseOutputAsync(status, statusDescription, tokenSource.Token).ContinueWith(OnCloseOutput);
                return new WebSocketTask(task, tokenSource);
            }
            else
            {
                return new WebSocketTask();
            }
        }

        /// <summary>
        /// Determines if not connecting.
        /// </summary>
        /// <returns></returns>
        protected bool NotConnecting()
        {
            return socket == null || socket.State != WebSocketState.Connecting;
        }

        /// <summary>
        /// Ensures the socket is connected.
        /// </summary>
        protected void EnsureConnection()
        {
            if (socket != null)
            {
                switch (socket.State)
                {
                    case WebSocketState.CloseReceived:
                    case WebSocketState.CloseSent:
                        break;
                    case WebSocketState.Aborted:
                    case WebSocketState.Closed:
                        Connect(socketUri);
                        break;
                    case WebSocketState.Connecting:
                    case WebSocketState.Open:
                        break;

                }
            }
        }

        /// <summary>
        /// Called when sent finished.
        /// </summary>
        /// <param name="obj">The sent task.</param>
        protected void OnSent(Task obj)
        {
            if (obj.Exception != null)
            {
                log.Error("Failed to send data.");
                for (int i = 0; i < obj.Exception.InnerExceptions.Count; i++)
                {
                    log.Error(obj.Exception.InnerExceptions[i]);
                }
            }
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <exception cref="InvalidOperationException">Cannot send using a disposed socket.</exception>
        public void Send(byte[] data)
        {
            if (socket == null)
            {
                throw new InvalidOperationException("Cannot send using a disposed socket.");
            }

            if (Debug)
            {
                log.ConditionalDebug("Sending {0}...", System.Text.Encoding.ASCII.GetString(data));
            }

            if (CanSend())
            {
                lock (socketSendLock)
                {
                    ArraySegment<byte> segment = new ArraySegment<byte>(data);

                    Task task = null;
                    try
                    {
                        task = socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                        task.Wait();
                    }
                    catch (AggregateException aex)
                    {
                        for (int i = 0; i < aex.InnerExceptions.Count; i++)
                        {
                            log.Error(aex.InnerExceptions[i], "(Internal) Data was not sent.");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex, "(Internal) Data was not sent.");
                    }
                }
            }
            else
            {
                sendQueue.Enqueue(data);
                log.Warn("Data was not sent. Socket is not connected. Socket is in the {0} state.", Enum.GetName(typeof(WebSocketState), socket.State));
            }
        }

        /// <summary>
        /// Handles a listener exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        protected void HandleListenerException(Exception ex)
        {
            if (ex == null)
            {
                return;
            }

            if (ex is OperationCanceledException)
            {
                log.ConditionalDebug("Connection gracefully closed.");
            }
            else if (ex is WebSocketException)
            {
                WebSocketException wse = ex as WebSocketException;
                WebSocketError code = wse.WebSocketErrorCode;

                if (wse.InnerException != null)
                {
                    HandleListenerException(wse.InnerException);
                }
                else if (code == WebSocketError.Success)
                {
                    log.ConditionalDebug("Connection gracefully closed.");
                }
                else
                {
                    string disconnectReason = Enum.GetName(typeof(WebSocketError), code);
                    log.ConditionalDebug("Connection closed: {0}", disconnectReason);
                }
            }
            else if (ex is ThreadAbortException)
            {
                log.ConditionalDebug("Listen thread was aborted successfully.");
            }
            else if (ex is AggregateException)
            {
                AggregateException ae = ex as AggregateException;
                for (int i = 0; i < ae.InnerExceptions.Count; i++)
                {
                    HandleListenerException(ae.InnerExceptions[i]);
                }
            }
            else
            {
                log.Fatal(ex, "Unhandled listen thread exception.");
            }
        }

        /// <summary>
        /// Determines whether this instance can send.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can send; otherwise, <c>false</c>.
        /// </returns>
        protected bool CanSend()
        {
            return socket != null && (socket.State == WebSocketState.Open || socket.State == WebSocketState.CloseReceived);
        }

        /// <summary>
        /// Determines whether this instance is connected.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </returns>
        public bool IsConnected()
        {
            return socket != null && (socket.State == WebSocketState.Open || socket.State == WebSocketState.CloseReceived);
        }

        /// <summary>
        /// Determines whether this instance is closed.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is closed; otherwise, <c>false</c>.
        /// </returns>
        private bool IsClosed()
        {
            return socket != null && (socket.State == WebSocketState.Closed || socket.State == WebSocketState.Aborted);
        }
    }
}
