using LilaSharp.Delegates;
using LilaSharp.Events;
using LilaSharp.Internal;
using LilaSharp.Messages;
using LilaSharp.Packets;
using LilaSharp.Types;
using NLog;
using System;

namespace LilaSharp
{
    /// <summary>
    /// Logic pertaining for a lichess game.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class LilaGame : IDisposable
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        private object disposeLock = new object();
        private int _version;
        private Random random;
        private PPingVersion gamePing;
        private GameData gameData;
        private LilaSocket socket;
        private ClockData curClock;

        /// <summary>
        /// Occurs when the game ends.
        /// </summary>
        public event LilaEventHandler<LilaGameEvent> OnGameEnd;

        /// <summary>
        /// Occurs when a game move is made.
        /// </summary>
        public event LilaEventHandler<LilaGameMoveEvent> OnGameMove;

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public LilaClient Client { get; private set; }

        /// <summary>
        /// Gets the clock.
        /// </summary>
        /// <value>
        /// The clock.
        /// </value>
        public ClockData Clock {  get { return curClock; } }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public GameData Data {  get { return gameData; } }

        /// <summary>
        /// Gets or sets the game version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version
        {
            get
            {
                return _version;
            }
            protected set
            {
                _version = value;
                gamePing.Version = value;
            }
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
            lock (disposeLock)
            {
                if (socket != null)
                {
                    Resign();
                    socket.Dispose();
                    socket = null;
                }

                if (finalize)
                {
                    GC.SuppressFinalize(this);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LilaGame"/> class.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="socket">The socket.</param>
        /// <param name="gameData">The game data.</param>
        internal LilaGame(LilaClient c, LilaSocket socket, GameData gameData)
        {
            gamePing = new PPingVersion(0);
            Version = gameData.Player.Version;
            curClock = new ClockData
            {
                White = gameData.Clock.White,
                Black = gameData.Clock.Black
            };

            Client = c;

            this.socket = socket;
            this.gameData = gameData;

            socket.OnDisconnect += OnDisconnect;

            socket.AddHandler<MPong>(OnPong);
            socket.AddHandler<MMove>(OnMove);
            socket.AddHandler<MGameMessages>(OnGameEvent);
            socket.AddHandler<MReload>(OnReload);
            socket.AddHandler<MMessage>(OnMessage);
            socket.AddHandler<MEnd>(OnEnd);
            socket.AddHandler<MEndData>(OnEndData);
            socket.AddHandler<MTakebackOffer>(OnTakebackOffer);
            socket.AddHandler<MAck>(OnAck);
            socket.AddHandler<MGone>(OnGone);
            socket.AddHandler<MCrowd>(OnCrowd);
            socket.AddHandler<MClock>(OnClock);
            socket.AddHandler<MChallenges>(OnChallenges);
            socket.AddHandler<MResync>(OnResync);
            socket.AddHandler<MRoundRedirect>(OnRedirect);
            socket.AddHandler<MCheckCount>(OnCheckCount);
            socket.AddHandler<MTournamentReminder>(OnReminder);
            socket.AddHandler<MBerserk>(OnBerserk);
            socket.AddHandler<MClockIncrement>(OnClockInc);
            socket.AddHandler<MTournamentStandings>(OnStandings);
            socket.AddHandler<MDeployPre>(OnDeployPre);

            socket.AddVersionHandler<MVersion>(OnVersion);

            SendPing();
            socket.SchedulePacket(gamePing, 1000);

            random = new Random();           
        }

        /// <summary>
        /// Processes the game message to update the game's version.
        /// </summary>
        /// <param name="message">The message containing version information.</param>
        /// <returns>True if updated; otherwise false.</returns>
        private bool ProcessGameMessage(IVersionedMessage message)
        {
            int v = message.Version;
            if (v == Version + 1) //Next version
            {
                Version = v;
                return true;
            }
            else if (v > Version + 1) //Skipped version?
            {
                //Queue up?
                return true;
            }

            return false;
        }

        /// <summary>
        /// Called when raw version is received.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnVersion(WebSocketBase ws, MVersion message)
        {
            ProcessGameMessage(message);
        }

        /// <summary>
        /// Called when the client is disconnected.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void OnDisconnect(object sender, SocketDisconnectArgs e)
        {
            log.ConditionalDebug("Disconnected from {0}", gameData.Url.Socket);
            if (socket != null && !e.Initiated && e.ReconnectionAttempts < Client.Settings.ReconnectionAttemptLimit)
            {
                log.ConditionalDebug("Reconnecting {0}", gameData.Url.Socket);
                socket.Reconnect();
            }
        }

        /// <summary>
        /// Called when socket needs to resync.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnResync(WebSocketBase ws, MResync message)
        {
            //TODO
        }

        /// <summary>
        /// Called when challenges are updated.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnChallenges(WebSocketBase ws, Messages.MChallenges message)
        {
            
        }

        /// <summary>
        /// Called when an message is ack'ed.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnAck(WebSocketBase ws, MAck message)
        {
            
        }

        /// <summary>
        /// Called when opponent leaves or rejoins game.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnGone(WebSocketBase ws, MGone message)
        {
        }

        /// <summary>
        /// Called when the crowd is updated.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnCrowd(WebSocketBase ws, MCrowd message)
        {
            
        }

        /// <summary>
        /// Called when a three-check game check count is updated.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnCheckCount(WebSocketBase ws, Messages.MCheckCount message)
        {
            ProcessGameMessage(message);
        }

        /// <summary>
        /// Called when a redirection is requested.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnRedirect(WebSocketBase ws, MRoundRedirect message)
        {
            if (ProcessGameMessage(message) && Client.Remove(this))
            {
                Client.JoinGame(message.Redirect.Id);         
            }
        }

        /// <summary>
        /// Called when the clock is updated.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnClock(WebSocketBase ws, Messages.MClock message)
        {
            ProcessGameMessage(message);
            curClock = message.Data;
        }

        /// <summary>
        /// Called when a takeback offer is received.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnTakebackOffer(WebSocketBase ws, MTakebackOffer message)
        {
            ProcessGameMessage(message);
        }

        /// <summary>
        /// Called when the game end data is received.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnEndData(WebSocketBase ws, Messages.MEndData message)
        {
            ProcessGameMessage(message);
        }

        /// <summary>
        /// Called when the game ends.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnEnd(WebSocketBase ws, MEnd message)
        {
            ProcessGameMessage(message);
            if (Client != null)
            {
                Client.Remove(this);
            }

            OnGameEnd?.Invoke(this, new LilaGameEvent(Client, this));
        }

        /// <summary>
        /// Called when the game needs to be reloaded.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnReload(WebSocketBase ws, MReload message)
        {
            if (ProcessGameMessage(message))
            {
                if (message.Data?.Type == "rematchOffer")
                {
                    AcceptRematch();
                }
            }
        }

        /// <summary>
        /// Called when tournament standings are updated.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnStandings(WebSocketBase ws, MTournamentStandings message)
        {
        }

        /// <summary>
        /// Called when a reminder is received.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnReminder(WebSocketBase ws, MTournamentReminder message)
        {
        }

        /// <summary>
        /// Called when lichess is restarting.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnDeployPre(WebSocketBase ws, MDeployPre message)
        {
            //No log here, logged from lobby connection
        }

        /// <summary>
        /// Called when a player berserks.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnBerserk(WebSocketBase ws, MBerserk message)
        {
            ProcessGameMessage(message);
        }

        /// <summary>
        /// Called when clock time is added.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnClockInc(WebSocketBase ws, Messages.MClockIncrement message)
        {
            ProcessGameMessage(message);
        }

        /// <summary>
        /// Called when a chat message is received.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnMessage(WebSocketBase ws, Messages.MMessage message)
        {
            ProcessGameMessage(message);
        }

        /// <summary>
        /// Called when a game event is received.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnGameEvent(WebSocketBase ws, MGameMessages message)
        {
            for (int i = 0; i < message.Data.Count; i++)
            {
                if (socket != null)
                {
                    socket.InvokeMessage(message.Data[i]);
                }
            }
        }

        /// <summary>
        /// Called when the game pings back.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnPong(WebSocketBase ws, MPong message)
        {
            TimeSpan lag = DateTime.Now.Subtract(gamePing.LastSent);
            Client.ClientLag = lag.TotalMilliseconds;
        }

        /// <summary>
        /// Called when a move is received.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnMove(WebSocketBase ws, MMove message)
        {
            ProcessGameMessage(message);
            curClock = message.Data.Clock;
            OnGameMove?.Invoke(this, new LilaGameMoveEvent(Client, this, message.Data));
        }

        /// <summary>
        /// Plays a move.
        /// </summary>
        /// <param name="move">The move.</param>
        public void PlayMove(PData<PMove> move)
        {
            if (socket != null)
            {
                socket.Send(move);
            }
        }

        /// <summary>
        /// Says the specified message in the chat.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Talk(string message)
        {
            if (socket != null)
            {
                socket.Send(new PTalk(message));
            }
        }

        /// <summary>
        /// Berserks the game.
        /// </summary>
        public void Berserk()
        {
            //TODO: add checks for tournament
            if (socket != null)
            {
                socket.SendType("berserk");
            }
        }

        /// <summary>
        /// Resigns the game.
        /// </summary>
        public void Resign()
        {
            if (socket != null)
            {
                socket.SendType("resign");
            }
        }

        /// <summary>
        /// Aborts the game.
        /// </summary>
        public void Abort()
        {
            if (socket != null)
            {
                socket.SendType("abort");
            }
        }

        /// <summary>
        /// Adds time to opponents clock.
        /// </summary>
        public void AddTime()
        {
            if (socket != null)
            {
                socket.SendType("moretime");
            }
        }

        /// <summary>
        /// Sends a ping.
        /// </summary>
        public void SendPing()
        {
            if (socket != null)
            {
                socket.Send(gamePing);
            }
        }

        /// <summary>
        /// Accepts a rematch.
        /// </summary>
        public void AcceptRematch()
        {
            if (socket != null)
            {
                socket.SendType("rematch-yes");
            }
        }

        /// <summary>
        /// Rejects a rematch.
        /// </summary>
        public void RejectRematch()
        {
            if (socket != null)
            {
                socket.SendType("rematch-no");
            }
        }
    }
}
