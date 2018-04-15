using HtmlAgilityPack;
using LilaSharp.Api;
using LilaSharp.Events;
using LilaSharp.Internal;
using LilaSharp.Messages;
using LilaSharp.Packets;
using LilaSharp.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace LilaSharp
{
    /// <summary>
    /// Client for connecting to lichess.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public sealed class LilaClient : IDisposable
    {
        //TODO: "Language subdomains are deprecated and will be discontinued" remove subdomains

        /// <summary>
        /// Statically initializes the <see cref="LilaClient"/> class
        /// </summary>
        static LilaClient()
        {
            //Very important will not work without increased limit
            ServicePointManager.DefaultConnectionLimit = 200;
        }

        private static Logger log = LogManager.GetCurrentClassLogger();

        private string challengeLocation;

        private bool _disposing = false;
        private PP _lobbyPing;
        private PP _challengePing;
        private PP _gamePing;
        private PPing _challengePing2;

        private LilaRandom random;
        private LilaSocket lobbyCon;     
        private LilaSocket challengeCon;

        private ConcurrentDictionary<string, LilaTournament> tournamentCons;
        private ConcurrentDictionary<string, LilaGame> gameCons;

        private bool anonymous = false;
        private LilaSettings lilaSettings;
        private JsonSerializerSettings jsonSettings;

        private object joinLock;
        private object hookLock;
        private List<IHook> hooks;

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>
        /// The culture.
        /// </value>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// Gets the client lag.
        /// </summary>
        /// <value>
        /// The client lag.
        /// </value>
        public double ClientLag { get; internal set; }

        /// <summary>
        /// Gets the server lag.
        /// </summary>
        /// <value>
        /// The server lag.
        /// </value>
        public int ServerLag { get; private set; }

        /// <summary>
        /// Gets the players.
        /// </summary>
        /// <value>
        /// The players.
        /// </value>
        public int Players { get; private set; }

        /// <summary>
        /// Gets the games.
        /// </summary>
        /// <value>
        /// The games.
        /// </value>
        public int Games { get; private set; }

        /// <summary>
        /// Gets the lobby sri.
        /// </summary>
        /// <value>
        /// The lobby sri.
        /// </value>
        public string LobbySri { get; private set; }

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <value>
        /// The events.
        /// </value>
        public LilaEvents Events { get; private set; }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public LilaSettings Settings
        {
            get
            {
                return lilaSettings;
            }
            set
            {
                if (value != null)
                {
                    lilaSettings = value;
                } 
            }
        }

        /// <summary>
        /// Gets the hooks.
        /// </summary>
        /// <value>
        /// The hooks.
        /// </value>
        public IReadOnlyList<IHook> Hooks { get { return hooks.AsReadOnly(); } }

        /// <summary>
        /// Gets the game connections.
        /// </summary>
        /// <value>
        /// The game connections.
        /// </value>
        public int GameConnections {  get { return gameCons.Count; } }

        /// <summary>
        /// Challenges a player.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="form">The form.</param>
        /// <returns></returns>
        public async Task<bool> ChallengePlayer(string username, GameForm form)
        {
            if (_disposing || challengeCon == null)
            {
                return false;
            }

            LilaRequest setupFriend = new LilaRequest(new Uri(string.Format("/setup/friend?user={0}", username.ToLower()), UriKind.Relative), Culture);
            setupFriend.Cookies.Add(lobbyCon.GetCookies());

            string[] keys = new string[] { "variant", "fen", "timeMode", "time", "increment", "days", "mode", "color" };
            object[] values = new object[] { form.Variant, form.Fen, form.TimeMode, form.Time, form.Increment, form.Days, form.Mode, form.Color };

            LilaResponse res = await setupFriend.Post(LilaRequest.ContentType.Any, keys, values);
            if (res == null || !res.CheckStatus(HttpStatusCode.OK | HttpStatusCode.SeeOther))
            {
                return false;
            }

            string loc = (challengeLocation = res.GetHeader("Location")).Trim('/');

            Uri host = new Uri("wss://socket.lichess.org");
            Uri relative = new Uri(string.Format("/challenge/{0}/socket/v2", loc), UriKind.Relative);
            Uri absolute = new Uri(host, relative);

            UriBuilder challengeBldr = new UriBuilder(absolute)
            {
                Query = string.Format("sri={0}", random.NextSri())
            };

            //int port = LilaPing.PingServer(9025, 9029, 1);
            //challengeBldr.Port = port == -1 ? 9025 : port;

            challengeCon.AddCookies(lobbyCon.GetCookies());
            if (challengeCon.Connect(challengeBldr.Uri))
            {
                log.ConditionalDebug("Connected to challenge socket. Awaiting reload message.");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Logs in as anonymous player.
        /// </summary>
        public void Login()
        {
            anonymous = true;
            LilaRequest req = new LilaRequest(new Uri(LilaRoutes.Lobby, UriKind.Relative), Culture);
            Task<LilaResponse> result = req.Get(LilaRequest.ContentType.Html);
            result.ContinueWith(HandleAuth);
        }

        /// <summary>
        /// Logs in as the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public void Login(string username, string password)
        {
            if (_disposing)
            {
                return;
            }

            anonymous = false;
            LilaRequest hpReq = new LilaRequest(new Uri(LilaRoutes.Lobby, UriKind.Relative), Culture);
            Task<LilaResponse> hpResTask = hpReq.Get(LilaRequest.ContentType.Any);
            hpResTask.Wait();

            LilaRequest req = new LilaRequest(new Uri(LilaRoutes.Login, UriKind.Relative), Culture);
            if (hpResTask.Result != null)
            {
                req.Cookies.Add(hpResTask.Result.GetCookies());
            }

            Task<LilaResponse> result = req.Post(LilaRequest.ContentType.Html, new string[] { "username", "password" }, new string[] { username, password });
            result.ContinueWith(HandleAuth);
        }

        /// <summary>
        /// Creates a seek.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="increment">The increment.</param>
        /// <param name="ratingLow">The rating low.</param>
        /// <param name="ratingHigh">The rating high.</param>
        /// <returns></returns>
        public async Task<SeekResponse> CreateSeek(int time, int increment, int ratingLow, int ratingHigh)
        {
            //TODO: Change for anonymous player requests
            
            LilaRequest setupSeek = new LilaRequest(new Uri(string.Format("/setup/hook/{0}", LobbySri), UriKind.Relative));
            setupSeek.Cookies.Add(lobbyCon.GetCookies());

            double timeMinutes = time / 60000.0;
            string ratingRange = string.Format("{0}-{1}", ratingLow, ratingHigh);

            string[] keys = new string[] { "variant", "timeMode", "time", "increment", "days", "mode", "ratingRange", "color" };
            object[] values = new object[] { 1, 1, timeMinutes, increment, 2, 1, ratingRange, "white" };

            LilaResponse res = await setupSeek.Post(LilaRequest.ContentType.Any, keys, values);
            if (res == null || !res.CheckStatus(HttpStatusCode.OK | HttpStatusCode.SeeOther))
            {
                return null;
            }

            SeekResponse sr = res.ReadJson<SeekResponse>();
            return sr;
        }

        /// <summary>
        /// Handles the authentication.
        /// </summary>
        /// <param name="callback">The callback.</param>
        private void HandleAuth(Task<LilaResponse> callback)
        {
            if (callback.IsFaulted || !callback.Result.CheckStatus(HttpStatusCode.OK | HttpStatusCode.SeeOther))
            {
                LilaEvent authFail = new LilaEvent(this);
                Events.FireEventAsync(Events._onAuthenticationFail, authFail);
                log.Warn("Events.OnAuthentication is not added, login will not connect to the lobby.");
            }
            else
            {
                //Authenticated
                GotoLobby(callback);
            }
        }

        /// <summary>
        /// Goes to the lobby.
        /// </summary>
        /// <param name="obj">The login response</param>
        private void GotoLobby(Task<LilaResponse> obj)
        {
            if (!obj.IsFaulted && obj.Result != null)
            {
                var result = obj.Result.GetCookies();
                lobbyCon.AddCookies(result);
            }

            ConnectLobbyInternal();
        }

        /// <summary>
        /// Connects the lobby internally
        /// </summary>
        private void ConnectLobbyInternal()
        {
            if (_disposing)
            {
                return;
            }

            Uri host = new Uri("wss://socket.lichess.org");
            Uri relative = new Uri(LilaRoutes.LobbySocket, UriKind.Relative);
            Uri absolute = new Uri(host, relative);

            UriBuilder lobbyBldr = new UriBuilder(absolute)
            {
                Query = string.Format("sri={0}", LobbySri = random.NextSri())
            };

            //int port = LilaPing.PingServer(9025, 9029, 1);
            //lobbyBldr.Port = port == -1 ? 9025 : port;

            if (lobbyCon != null && lobbyCon.Connect(lobbyBldr.Uri))
            {
                Events.FireEventAsync(Events._onConnect, new ConnectEvent(this));

                lobbyCon.Send(new PHookIn());
                lobbyCon.Send(new PServerLatency(true));
            }
        }

        /// <summary>
        /// Joins a tournament.
        /// </summary>
        /// <param name="id">The tournament identifier.</param>
        /// <returns>True if successful; otherwise False</returns>
        public bool JoinTournament(string id)
        {
            string tournamentPath = id;
            if (tournamentPath.StartsWith("/"))
            {
                tournamentPath = tournamentPath.Substring(1);
            }

            Task<Tournament> task = GetTournament(tournamentPath);
            task.Wait();

            if (task.Result != null)
            {
                return JoinTournament(task.Result.Data);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Leaves a tournament.
        /// </summary>
        /// <param name="id">The tournament identifier.</param>
        public void LeaveTournament(string id)
        {
            if (tournamentCons.TryRemove(id, out LilaTournament t))
            {
                t.Dispose();
                if (!lobbyCon.IsConnected())
                {
                    lobbyCon.Reconnect();
                }
            }
        }

        /// <summary>
        /// Leaves a tournament.
        /// </summary>
        /// <param name="tournament">The tournament.</param>
        public void LeaveTournament(LilaTournament tournament)
        {
            tournamentCons.TryRemove(tournament.Data.Id, out LilaTournament t);
            tournament.Dispose();

            if (!lobbyCon.IsConnected())
            {
                lobbyCon.Reconnect();
            }
        }

        /// <summary>
        /// Joins a tournament.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        internal bool JoinTournament(TournamentData data)
        {
            if (_disposing)
            {
                return false;
            }

            Uri host = new Uri("wss://socket.lichess.org");

            if (!Uri.TryCreate(string.Format("/tournament/{0}/socket/v2", data.Id), UriKind.Relative, out Uri relative))
            {
                return false;
            }

            Uri absolute = new Uri(host, relative);
            if (random == null)
            {
                return false;
            }

            UriBuilder gameBldr = new UriBuilder(absolute)
            {
                Query = string.Format("sri={0}", random.NextSri())
            };

            LilaSocket tournamentCon = new LilaSocket("Tournament-Socket", ResourceType.Thread);
            tournamentCon.AddCookies(lobbyCon.GetCookies());

            if (tournamentCon.Connect(gameBldr.Uri) && !_disposing)
            {
                //Disconnect from lobby
                if (lobbyCon.IsConnected())
                {
                    lobbyCon.Disconnect();
                }

                LilaTournament lilaTournament = new LilaTournament(this, tournamentCon, data);
                tournamentCons.TryAdd(data.Id, lilaTournament);

                Events.FireEventAsync(Events._onTournamentEnter, new TournamentEvent(this, lilaTournament));
                return true;
            }

            tournamentCon.Dispose();
            return false;
        }

        /// <summary>
        /// Joins a game.
        /// </summary>
        /// <param name="hook">The hook.</param>
        public void JoinGame(IHook hook)
        {
            if (_disposing)
            {
                return;
            }

            lock (joinLock)
            {
                if (gameCons.Count == 0 && lobbyCon != null) //Limit games to one for now
                {
                    lobbyCon.Send(new PJoin(hook.ID));
                }
            }
        }

        /// <summary>
        /// Leaves a game.
        /// </summary>
        /// <param name="game">The game.</param>
        public void LeaveGame(LilaGame game)
        {
            Remove(game);
        }

        /// <summary>
        /// Joins a game.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <returns></returns>
        internal bool JoinGame(GameData game)
        {
            if (_disposing)
            {
                return false;
            }

            lock (joinLock)
            {
                Uri host = new Uri("wss://socket.lichess.org");
                if (!Uri.TryCreate(game.Url.Socket, UriKind.Relative, out Uri relative))
                {
                    return false;
                }

                Uri absolute = new Uri(host, relative);
                if (random == null)
                {
                    return false;
                }

                UriBuilder gameBldr = new UriBuilder(absolute)
                {
                    Query = string.Format("sri={0}", random.NextSri())
                };

                LilaSocket gameCon = new LilaSocket("Game-Socket", ResourceType.Thread);
                gameCon.AddCookies(lobbyCon.GetCookies());
                if (anonymous)
                {
                    gameCon.AddCookies(new Cookie("rk2", game.Url.Socket.Substring(9, 4), "/", "lichess.org")); //Add anoncookie
                }

                if (gameCons.Count == 0 && gameCon.Connect(gameBldr.Uri) && !_disposing)
                {
                    LilaGame lilaGame = new LilaGame(this, gameCon, game);
                    gameCons.TryAdd(game.Url.Socket, lilaGame);

                    Events.FireEventAsync(Events._onJoinGame, new JoinGameEvent(this, lilaGame));
                    return true;
                }

                gameCon.Dispose();
                return false;
            }
        }

        /// <summary>
        /// Joins a game.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        internal bool JoinGame(string id)
        {
            string roundPath = id;
            if (!roundPath.StartsWith("/"))
            {
                roundPath = roundPath.Insert(0, "/");
            }

            Task<Round> task = GetRound(roundPath);
            task.Wait();

            if (task.Result != null)
            {
                return JoinGame(task.Result.Game);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes the specified game.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <returns></returns>
        internal bool Remove(LilaGame game)
        {
            log.ConditionalDebug("Removing {0}", game.Data.Url.Socket);

            bool result = gameCons.TryRemove(game.Data.Url.Socket, out LilaGame s);
            game.Dispose();
            return result;
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
        public void Dispose(bool finalize)
        {
            _disposing = true;
            if (random != null)
            {
                random.Dispose();
                random = null;
            }

            if (lobbyCon != null)
            {
                lobbyCon.Dispose();
                lobbyCon = null;
            }

            if (challengeCon != null)
            {
                challengeCon.Dispose();
                challengeCon = null;
            }

            if (gameCons != null)
            {
                foreach (KeyValuePair<string, LilaGame> game in gameCons)
                {
                    log.ConditionalDebug("Disconnecting {0}", game.Key);
                    game.Value.Dispose();
                }

                gameCons = null;
            }

            if (tournamentCons != null)
            {
                foreach (KeyValuePair<string, LilaTournament> tournament in tournamentCons)
                {
                    log.ConditionalDebug("Disconnecting {0}", tournament.Key);
                    tournament.Value.Dispose();
                }

                tournamentCons = null;
            }

            if (finalize)
            {
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LilaClient"/> class.
        /// </summary>
        public LilaClient() : this(CultureInfo.CurrentCulture, LilaSettings.Default)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LilaClient"/> class.
        /// </summary>
        /// <param name="culture">The culture of the client.</param>
        public LilaClient(CultureInfo culture) : this(culture, LilaSettings.Default)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LilaClient"/> class.
        /// </summary>
        /// <param name="settings">The settings for the client to use.</param>
        public LilaClient(LilaSettings settings) : this(CultureInfo.CurrentCulture, settings)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LilaClient"/> class.
        /// </summary>
        /// <param name="culture">The culture of the client.</param>
        /// <param name="settings">The settings for the client to use.</param>
        public LilaClient(CultureInfo culture, LilaSettings settings)
        {
            random = new LilaRandom();
            tournamentCons = new ConcurrentDictionary<string, LilaTournament>();
            gameCons = new ConcurrentDictionary<string, LilaGame>();

            //#Lobby Connection
            lobbyCon = new LilaSocket("Lobby-Socket", ResourceType.Thread);
            lobbyCon.OnDisconnect += OnLobbyDisconnect;

            //#Challenge Connection
            challengeCon = new LilaSocket("Challenge-Socket", ResourceType.Task);
            challengeCon.OnDisconnect += OnChallengeDisconnect;

            //#Hooks
            joinLock = new object();
            hookLock = new object();
            hooks = new List<IHook>();

            //#Json
            jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                Error = OnJsonParseError,
                ContractResolver = new PacketResolver()
            };

            //#Packets
            _lobbyPing = new PP();
            _challengePing = new PP();
            _challengePing2 = new PPing();
            _gamePing = new PP();

            //#Lobby Events
            Events = new LilaEvents();
            lobbyCon.AddHandler<MPong>(OnLobbyPong);
            lobbyCon.AddHandler<MServerLatency>(OnLag);
            lobbyCon.AddHandler<MRemovedHooks>(OnHooksRemoved);
            lobbyCon.AddHandler<MNewHook>(OnNewHook);
            lobbyCon.AddHandler<MHookSync>(OnHookSync);
            lobbyCon.AddHandler<MHooks>(OnHooks);
            lobbyCon.AddHandler<MReloadSeeks>(OnReloadSeeks);
            lobbyCon.AddHandler<MRoundRedirect>(OnRedirect);
            lobbyCon.AddHandler<MChallenges>(OnChallenges);
            lobbyCon.AddHandler<MTournaments>(OnTournaments);
            lobbyCon.AddHandler<MSimuls>(OnSimuls);
            lobbyCon.AddHandler<MStreams>(OnStreams);
            lobbyCon.AddHandler<MFeatured>(OnFeatured);
            lobbyCon.AddHandler<MTournamentReminder>(OnReminder);
            lobbyCon.AddHandler<MReloadForum>(OnReloadForum);
            lobbyCon.AddHandler<MReloadTimeline>(OnReloadTimeline);
            lobbyCon.AddHandler<MDeployPre>(OnDeployPre);
            lobbyCon.AddHandler<MFollowingPlaying>(OnFollowingPlaying);
            lobbyCon.AddHandler<MFollowingStoppedPlaying>(OnFollowingStoppedPlaying);
            lobbyCon.AddHandler<MFollowingOnlines>(OnFollowingOnline);

            challengeCon.AddHandler<MPong>(OnChallengePong);
            challengeCon.AddHandler<MChallenges>(OnChallenges);
            challengeCon.AddHandler<MReload>(OnChallengeReload);

            //#Scheduled packets
            lobbyCon.SchedulePacket(_lobbyPing, 1000);         
            challengeCon.SchedulePacket(_challengePing, 1000);
            challengeCon.SchedulePacket(_challengePing2, 2000);

            if (culture == null)
            {
                Culture = CultureInfo.CurrentCulture;
            }
            else
            {
                Culture = culture;
            }

            if (settings == null)
            {
                lilaSettings = new LilaSettings();
            }
            else
            {
                lilaSettings = settings;
            }
        }

        /// <summary>
        /// Gets the round.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        private async Task<Round> GetRound(string location)
        {
            if (_disposing)
            {
                return null;
            }

            LilaRequest roundReq = new LilaRequest(new Uri(location, UriKind.Relative), Culture);
            roundReq.Cookies.Add(lobbyCon.GetCookies());
            if (anonymous)
            {
                roundReq.Cookies.Add(new Cookie("rk2", location.Substring(9), "/", "lichess.org")); //add anoncookie
            }

            LilaResponse roundRes = await roundReq.Get(LilaRequest.ContentType.Html);
            if (roundRes == null)
            {
                log.Error("Failed to join round at {0}", location);
                return null;
            }

            string html = await roundRes.ReadAsync();

            const string id = "LichessRound.boot(";

            int roundBootIndex = html.IndexOf(id);
            if (roundBootIndex == -1)
            {
                return null;
            }

            string json = StringEngine.GetInside(html, roundBootIndex + id.Length);
            if (json == null)
            {
                log.Error("Failed to parse game json data.");
                return null;
            }

            return JsonConvert.DeserializeObject<Round>(json, jsonSettings);
        }

        /// <summary>
        /// Gets the tournament.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <returns></returns>
        private async Task<Tournament> GetTournament(string tournamentId)
        {
            if (_disposing)
            {
                return null;
            }

            LilaRequest tournamentReq = new LilaRequest(new Uri(string.Format("/tournament/{0}", tournamentId), UriKind.Relative), Culture);
            tournamentReq.Cookies.Add(lobbyCon.GetCookies());

            LilaResponse roundRes = await tournamentReq.Get(LilaRequest.ContentType.Html);
            if (roundRes == null)
            {
                log.Error("Failed to join tournament at {0}", tournamentId);
                return null;
            }

            string html = await roundRes.ReadAsync();

            const string id = "lichess.tournament = ";

            int roundBootIndex = html.IndexOf(id);
            if (roundBootIndex == -1)
            {
                return null;
            }

            string json = StringEngine.GetInside(html, roundBootIndex + id.Length);
            if (json == null)
            {
                log.Error("Failed to parse tournament json data.");
                return null;
            }

            return JsonConvert.DeserializeObject<Tournament>(json, jsonSettings);
        }

        /// <summary>
        /// Called when the client is disconncted from the lobby.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void OnLobbyDisconnect(object sender, SocketDisconnectArgs e)
        {
            log.ConditionalDebug("Disconnected from lobby socket.");
            if (lobbyCon != null && !_disposing && !e.Initiated && Settings.AutoReconnect && e.ReconnectionAttempts < Settings.ReconnectionAttemptLimit)
            {
                Events.FireEventAsync(Events._onDisconnect, new Events.DisconnectEvent(this));

                if (Settings.AutoReconnect)
                {
                    log.ConditionalDebug("Reconnecting.");
                    lobbyCon.Reset();
                    ConnectLobbyInternal();
                }
            }
        }

        /// <summary>
        /// Called when challenge socket is disconnected.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void OnChallengeDisconnect(object sender, SocketDisconnectArgs e)
        {
            log.ConditionalDebug("Disconnected from challenge socket.");
            if (challengeCon != null && !_disposing && !e.Initiated && e.ReconnectionAttempts < Settings.ReconnectionAttemptLimit)
            {
                log.ConditionalDebug("Reconnecting challenge socket.");
                challengeCon.Reconnect();
            }
        }

        /// <summary>
        /// Called when challenges are updated.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnChallenges(WebSocketBase ws, MChallenges message)
        {
            Events.FireEventAsync(Events._onChallenges, new ChallengesEvent(this, message.Challenges));
        }

        /// <summary>
        /// Called when a redirection is requested.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnRedirect(WebSocketBase ws, MRoundRedirect message)
        {
            if (message.Redirect != null)
            {
                JoinGame(message.Redirect.ID);
            }
        }

        /// <summary>
        /// Called when a challenge is reloaded.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnChallengeReload(WebSocketBase ws, MReload message)
        {
            if (_disposing)
            {
                return;
            }

            if (challengeCon != null && challengeCon.IsConnected())
            {
                //Disconnect challenge socket
                log.ConditionalDebug("Disconnecting challenge socket.");
                challengeCon.DisconnectAsync().Wait();          
            }

            if (challengeLocation != null)
            {
                Task<Round> roundTask = GetRound(challengeLocation);
                roundTask.Wait();

                Round round = roundTask.Result;
                if (round != null)
                {
                    if (gameCons.TryGetValue(round.Game.Url.Socket, out LilaGame gameCon))
                    {
                        return;
                    }

                    JoinGame(round.Game);
                }
            }
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
        /// Called when a following stops playing.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnFollowingStoppedPlaying(WebSocketBase ws, MFollowingStoppedPlaying message)
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
        /// Called when tournaments are updated.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnTournaments(WebSocketBase ws, MTournaments message)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(message.Html);

            List<TournamentHtmlEntry> ids = new List<TournamentHtmlEntry>();

            var docNode = doc.DocumentNode;
            if (docNode.ChildNodes.Count >= 2)
            {
                var tableEntries = docNode.ChildNodes[1].ChildNodes;
                for (int i = 0; i < tableEntries.Count; i++)
                {
                    if (tableEntries[i].Name == "tr" && tableEntries[i].ChildNodes.Count >= 2 && tableEntries[i].ChildNodes[1].ChildNodes.Count >= 2)
                    {
                        var hrefNode = tableEntries[i].ChildNodes[1].ChildNodes[1];
                        string tournamentLocation = hrefNode.GetAttributeValue("href", null);
                        if (tournamentLocation != null)
                        {
                            int sIndex = tournamentLocation.LastIndexOf('/');
                            if (sIndex != -1)
                            {
                                string tournamentId = tournamentLocation.Substring(sIndex + 1);
                                string tournamentName = hrefNode.InnerText.Trim();
                                TournamentHtmlEntry entry = new TournamentHtmlEntry
                                {
                                    Id = tournamentId,
                                    Name = tournamentName
                                };

                                ids.Add(entry);
                            }
                        }                      
                    }
                }
            }

            message.Tournaments = ids;
            Events.FireEventAsync(Events._onTournaments, new TournamentsEvent(this, ids));
        }

        /// <summary>
        /// Called when featured events are updated.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnFeatured(WebSocketBase ws, MFeatured message)
        {
        }

        /// <summary>
        /// Called when streams are updated.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnStreams(WebSocketBase ws, MStreams message)
        {  
        }

        /// <summary>
        /// Called when simuls are updated.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnSimuls(WebSocketBase ws, MSimuls message)
        {
        }

        /// <summary>
        /// Called when a parse error is caught.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ErrorEventArgs"/> instance containing the event data.</param>
        private void OnJsonParseError(object sender, ErrorEventArgs e)
        {
            log.Error(e.ErrorContext.Error, "Failed to deserialize json.");
        }

        /// <summary>
        /// Sets the hooks.
        /// </summary>
        /// <param name="response">The response.</param>
        private void SetHooks(Task<LilaResponse> response)
        {
            ServerHook[] hookList = response.Result.ReadJson<ServerHook[]>();
            if (hookList != null)
            {
                lock (hookLock)
                {
                    hooks.Clear();
                    for (int i = 0; i < hookList.Length; i++)
                    {
                        hooks.Add(hookList[i]);
                        Events.FireEventAsync(Events._onNewHook, new HookEvent(this, hookList[i]));
                    }
                }
            }
        }

        /// <summary>
        /// Called when seeks are reloaded.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnReloadSeeks(WebSocketBase ws, MReloadSeeks message)
        {
            LilaRequest lr = new LilaRequest(new Uri(LilaRoutes.LobbySeeks, UriKind.Relative), Culture);
            lr.Cookies.Add(lobbyCon.GetCookies());
            lr.Get(LilaRequest.ContentType.Json).ContinueWith(SetHooks);
        }

        /// <summary>
        /// Called when the timeline is reloaded.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnReloadTimeline(WebSocketBase ws, MReloadTimeline message)
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
            log.Debug("!!! Lichess will restart soon !!!");
        }

        /// <summary>
        /// Called when the forum needs to be reloaded.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnReloadForum(WebSocketBase ws, MReloadForum message)
        {
        }

        /// <summary>
        /// Called when hooks are updated.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnHooks(WebSocketBase ws, MHooks message)
        {
            lock (hookLock)
            {
                hooks.Clear();
                for (int i = 0; i < message.Hooks.Length; i++)
                {
                    hooks.Add(message.Hooks[i]);
                    Events.FireEventAsync(Events._onNewHook, new HookEvent(this, message.Hooks[i]));
                }
            }
        }

        /// <summary>
        /// Called when hooks need to be synchronized.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnHookSync(WebSocketBase ws, MHookSync message)
        {
            lock (hookLock)
            {
                string[] sync = message.GetSynchronized();
                for (int i = hooks.Count - 1; i >= 0; i--)
                {
                    bool remove = true;
                    for (int j = 0; j < sync.Length; j++)
                    {
                        if (hooks[i].ID.Equals(sync[j]))
                        {
                            remove = false;
                            break;
                        }
                    }

                    if (remove)
                    {
                        hooks.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// Called when a new hook is created.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnNewHook(WebSocketBase ws, MNewHook message)
        {
            lock (hookLock)
            {
                hooks.Add(message.Hook);
                Events.FireEventAsync(Events._onNewHook, new HookEvent(this, message.Hook));
            }
        }

        /// <summary>
        /// Called when hooks are removed.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnHooksRemoved(WebSocketBase ws, MRemovedHooks message)
        {
            lock (hookLock)
            {
                string[] ids = message.GetRemoved();
                for (int i = hooks.Count - 1; i >= 0; i--)
                {
                    bool remove = false;
                    for (int j = 0; j < ids.Length; j++)
                    {
                        if (hooks[i].ID.Equals(ids[j]))
                        {
                            remove = true;
                            break;
                        }
                    }

                    if (remove)
                    {
                        hooks.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// Called when the server latency is received.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnLag(WebSocketBase ws, MServerLatency message)
        {
            ServerLag = message.Latency;
        }

        /// <summary>
        /// Called when the lobby pings back.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnLobbyPong(WebSocketBase ws, MPong message)
        {
            TimeSpan lag = DateTime.Now.Subtract(_lobbyPing.LastSent);
            ClientLag = lag.TotalMilliseconds;
            Players = message.Players;
            Games = message.Games;
        }

        /// <summary>
        /// Called when challenge socket pings back.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnChallengePong(WebSocketBase ws, MPong message)
        {
            TimeSpan lag = DateTime.Now.Subtract(_challengePing.LastSent);
            ClientLag = lag.TotalMilliseconds;
            Players = message.Players;
            Games = message.Games;
        }

        /// <summary>
        /// Called when game pings back.
        /// </summary>
        /// <param name="ws">The websocket.</param>
        /// <param name="message">The message.</param>
        private void OnGamePong(WebSocketBase ws, MPong message)
        {
            TimeSpan lag = DateTime.Now.Subtract(_gamePing.LastSent);
            ClientLag = lag.TotalMilliseconds;
            Players = message.Players;
            Games = message.Games;
        }
    }
}
