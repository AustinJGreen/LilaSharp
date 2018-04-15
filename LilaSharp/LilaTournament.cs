using LilaSharp.Events;
using LilaSharp.Internal;
using LilaSharp.Messages;
using LilaSharp.Packets;
using LilaSharp.Types;
using NLog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace LilaSharp
{
    /// <summary>
    /// Lichess tournament implementation
    /// </summary>
    public class LilaTournament
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        private int _version;
        private PPingVersion tournamentPing;
        private TournamentData tournamentData;
        private LilaSocket socket;

        /// <summary>
        /// Gets or sets the version.
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
            set
            {
                _version = value;
                tournamentPing.Version = value;
            }
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public LilaClient Client { get; private set; }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public TournamentData Data { get { return tournamentData; } }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            try
            {
                Pause().Wait(1000);
            }
            finally
            {
                if (socket != null)
                {
                    socket.Dispose();
                    socket = null;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LilaTournament"/> class.
        /// </summary>
        /// <param name="c">The client.</param>
        /// <param name="socket">The socket.</param>
        /// <param name="tournamentData">The tournament data.</param>
        internal LilaTournament(LilaClient c, LilaSocket socket, TournamentData tournamentData)
        {
            tournamentPing = new PPingVersion(0);
            Version = tournamentData.SocketVersion;

            Client = c;

            this.socket = socket;
            this.tournamentData = tournamentData;

            socket.OnDisconnect += OnDisconnect;

            socket.AddHandler<MPong>(OnPong);
            socket.AddHandler<MCrowd>(OnCrowd);
            socket.AddHandler<MReload>(OnReload);
            socket.AddHandler<MMessage>(OnMessage);
            socket.AddHandler<MTournamentRedirect>(OnRedirect);
            socket.AddHandler<MTournamentReminder>(OnReminder);
            socket.AddHandler<MResync>(OnResync);
            socket.AddHandler<MChallenges>(OnChallenges);
            socket.AddHandler<MDeployPre>(OnDeployPre);
            socket.AddHandler<MFollowingPlaying>(OnFollowingPlaying);
            socket.AddHandler<MFollowingStoppedPlaying>(OnFollowingStoppedPlaying);
            socket.AddHandler<MFollowingOnlines>(OnFollowingOnline);

            SendPing();
            socket.SchedulePacket(tournamentPing, 1000);
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
        /// Called when the tournament socket disconnects.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void OnDisconnect(object sender, SocketDisconnectArgs e)
        {
            log.ConditionalDebug("Disconnected from /tournament/{0}", tournamentData.Id);
            if (socket != null && !e.Initiated && e.ReconnectionAttempts < Client.Settings.ReconnectionAttemptLimit)
            {
                log.ConditionalDebug("Reconnecting /tournament/{0}", tournamentData.Id);
                socket.Reconnect();
            }
        }

        /// <summary>
        /// Called when the crowd is updated.
        /// </summary>
        /// <param name="ws">The ws.</param>
        /// <param name="message">The message.</param>
        private void OnCrowd(WebSocketBase ws, MCrowd message)
        {
        }

        /// <summary>
        /// Called when the game needs to be reloaded.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnReload(WebSocketBase ws, MReload message)
        {
            ProcessGameMessage(message);
        }

        /// <summary>
        /// Called when socket needs to resync.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnResync(WebSocketBase ws, MResync message)
        {
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
        /// Called when a redirection is requested.
        /// </summary>
        /// <param name="ws">The ws.</param>
        /// <param name="message">The message.</param>
        private void OnRedirect(WebSocketBase ws, MTournamentRedirect message)
        {
            Client.JoinGame(message.Id);
        }

        /// <summary>
        /// Called when the game pings back.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnPong(WebSocketBase ws, MPong message)
        {
            TimeSpan lag = DateTime.Now.Subtract(tournamentPing.LastSent);
            Client.ClientLag = lag.TotalMilliseconds;
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
            log.Debug("!!! Lichess will restart soon !!!");
        }

        /// <summary>
        /// Called when a following stops playing.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnFollowingStoppedPlaying(WebSocketBase ws, MFollowingStoppedPlaying message)
        {
        }

        /// <summary>
        /// Called when a following is playing.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnFollowingPlaying(WebSocketBase ws, MFollowingPlaying message)
        {
        }

        /// <summary>
        /// Called when a following is online.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnFollowingOnline(WebSocketBase ws, MFollowingOnlines message)
        {
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
        /// Sends a ping.
        /// </summary>
        public void SendPing()
        {
            if (socket != null)
            {
                socket.Send(tournamentPing);
            }
        }

        /// <summary>
        /// Sends a chat message.
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
        /// Joins the tournament (starts playing).
        /// </summary>
        public async Task<bool> Join()
        {
            LilaRequest joinReq = new LilaRequest(new Uri(string.Format("/tournament/{0}/join", tournamentData.Id), UriKind.Relative));
            joinReq.Cookies.Add(socket.GetCookies());

            //p is password
            string str = "{\"p\":null}";
            LilaResponse joinRes = await joinReq.Post(LilaRequest.ContentType.Json, str);
            
            bool success = joinRes != null && joinRes.CheckStatus(HttpStatusCode.OK | HttpStatusCode.SeeOther);
            if (success)
            {
                Client.Events.FireEventAsync(Client.Events._onTournamentJoin, new TournamentEvent(Client, this));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Pauses or withdraws the client from the tournament.
        /// </summary>
        public async Task<bool> Pause()
        {
            LilaRequest joinReq = new LilaRequest(new Uri(string.Format("/tournament/{0}/withdraw", tournamentData.Id), UriKind.Relative));
            joinReq.Cookies.Add(socket.GetCookies());

            //TODO: Add password
            //p is password
            string str = "{\"p\":null}";
            LilaResponse pauseRes = await joinReq.Post(LilaRequest.ContentType.Json, str);

            bool success = pauseRes != null && pauseRes.CheckStatus(HttpStatusCode.OK | HttpStatusCode.SeeOther);
            return success;
        }
    }
}
