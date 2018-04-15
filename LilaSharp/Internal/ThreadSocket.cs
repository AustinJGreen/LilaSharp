using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace LilaSharp.Internal
{
    /// <summary>
    /// Thread-based implementation of <see cref="LilaSharp.Internal.WebSocketBase"/>.
    /// </summary>
    /// <seealso cref="LilaSharp.Internal.WebSocketBase" />
    internal class ThreadSocket : WebSocketBase
    {
        private object recvLock = new object();
        private Thread listenThread;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="finalize"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool finalize)
        {
            if (finalize)
            {
                GC.SuppressFinalize(this);
            }

            base.Dispose(finalize);
        }

        /// <summary>
        /// Listening thread function.
        /// </summary>
        private void ListenThread()
        {
            recvToken = new CancellationTokenSource();

            byte[] buffer = new byte[_recvBufferSize];
            ArraySegment<byte> bufferSegment = new ArraySegment<byte>(buffer);

            bool close = false;
            Message m = new Message();

            try
            {
                while (IsConnected())
                {
                    Task<WebSocketReceiveResult> recvTask = null;
                    lock (recvLock)
                    {
                        recvTask = socket.ReceiveAsync(bufferSegment, recvToken.Token);
                        recvTask.Wait(recvToken.Token);
                    }

                    WebSocketReceiveResult recvResult = recvTask.Result;
                    if (recvResult != null)
                    {
                        switch (recvResult.MessageType)
                        {
                            case WebSocketMessageType.Binary:
                                log.Warn("Received unknown binary websocket message.");
                                break;
                            case WebSocketMessageType.Text:
                                m.Append(buffer, recvResult.Count);
                                break;
                            case WebSocketMessageType.Close:
                                close = true;
                                break;
                        }

                        if (close)
                        {
                            break;
                        }

                        if (recvResult.EndOfMessage)
                        {
                            if (!m.Empty)
                            {
                                reconnectionAttempts = 0;
                                HandleMessage(m);
                            }

                            m.Delete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleListenerException(ex);
            }

            if (recvToken != null)
            {
                recvToken.Dispose();
                recvToken = null;
            }

            if (!close && IsConnected())
            {
                CloseInternal();
            }
            else if (OnDisconnect != null)
            {
                StopSchedulers();
                SocketDisconnectArgs dea = new SocketDisconnectArgs(disconnectInitiated, reconnectionAttempts);
                try
                {
                    OnDisconnect.BeginInvoke(this, dea, OnDisconnectCallback, null);
                }
                catch (Exception odEr)
                {
                    log.Error(odEr, "Error in OnDisconnect event.");
                }
            }
            else
            {
                disconnectInitiated = false;
            }
        }

        /// <summary>
        /// Called on disconnect callback.
        /// </summary>
        /// <param name="ar">The ar.</param>
        protected void OnDisconnectCallback(IAsyncResult ar)
        {
            if (OnDisconnect != null)
            {
                try
                {
                    OnDisconnect.EndInvoke(ar);
                }
                catch (Exception ex)
                {
                    log.Error(ex, "An error asynchronously occurred in OnDisconnect.");
                }
            }
        }

        /// <summary>
        /// Starts listening.
        /// </summary>
        protected override void StartListening()
        {
            if (IsConnected())
            {
                StopListening();
                EnsureConnection();

                listenThread = new Thread(ListenThread)
                {
                    Name = "LilaSocket",
                    IsBackground = true,
                    Priority = ThreadPriority.Highest
                };

                listenThread.Start();
            }
        }

        /// <summary>
        /// Stops listening.
        /// </summary>
        protected override void StopListening()
        {
            if (listenThread != null)
            {
                switch (listenThread.ThreadState)
                {
                    case ThreadState.WaitSleepJoin:
                    case ThreadState.Background:
                    case ThreadState.Running:
                        if (recvToken != null)
                        {
                            recvToken.Cancel(false);
                        }
                        else if (IsConnected())
                        {
                            log.Warn("Listening thread is running without a cancellation token.");
                        }

                        if (!listenThread.Join(ReceiveTimeout))
                        {
                            log.Warn("Listening thread timed out. Aborting thread.");
                            listenThread.Abort();
                        }

                        break;
                    case ThreadState.Aborted:
                    case ThreadState.Stopped:
                    case ThreadState.Suspended:
                        break;
                   
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSocket"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ThreadSocket(string name) : base(name)
        {
            
        }
    }
}
