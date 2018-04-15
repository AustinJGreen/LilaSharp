using LilaSharp.Delegates;
using LilaSharp.Events;
using System;
using System.Threading.Tasks;

namespace LilaSharp
{
    /// <summary>
    /// Lichess events pertaining to <see cref="LilaClient"/>.
    /// </summary>
    public class LilaEvents
    {
        private object _onAuthenticationFailLock;
        internal LilaEventHandler<LilaEvent> _onAuthenticationFail;

        /// <summary>
        /// Occurs when a login is being authenticated.
        /// </summary>
        public event LilaEventHandler<LilaEvent> OnAuthenticationFail
        {
            add
            {
                lock (_onAuthenticationFailLock)
                {
                    _onAuthenticationFail += value;
                }
            }
            remove
            {
                lock (_onAuthenticationFailLock)
                {
                    _onAuthenticationFail -= value;
                }
            }
        }

        private object _onDisconnectLock;
        internal LilaEventHandler<Events.DisconnectEvent> _onDisconnect;

        /// <summary>
        /// Occurs when the client is disconnected.
        /// </summary>
        public event LilaEventHandler<Events.DisconnectEvent> OnDisconnect
        {
            add
            {
                lock (_onDisconnectLock)
                {
                    _onDisconnect += value;
                }
            }
            remove
            {
                lock (_onDisconnectLock)
                {
                    _onDisconnect -= value;
                }
            }
        }

        private object _onConnectLock;
        internal LilaEventHandler<ConnectEvent> _onConnect;

        /// <summary>
        /// Occurs when the client connects.
        /// </summary>
        public event LilaEventHandler<ConnectEvent> OnConnect
        {
            add
            {
                lock (_onConnectLock)
                {
                    _onConnect += value;
                }
            }
            remove
            {
                lock (_onConnectLock)
                {
                    _onConnect -= value;
                }
            }
        }

        private object _onNewHookLock;
        internal LilaEventHandler<HookEvent> _onNewHook;

        /// <summary>
        /// Occurs when a new hook/seek is added.
        /// </summary>
        public event LilaEventHandler<HookEvent> OnNewHook
        {
            add
            {
                lock (_onNewHookLock)
                {
                    _onNewHook += value;
                }
            }
            remove
            {
                lock (_onNewHookLock)
                {
                    _onNewHook -= value;
                }
            }
        }

        private object _onChallengesLock;
        internal LilaEventHandler<ChallengesEvent> _onChallenges;

        /// <summary>
        /// Occurs when challenges are updated.
        /// </summary>
        public event LilaEventHandler<ChallengesEvent> OnChallenges
        {
            add
            {
                lock (_onChallengesLock)
                {
                    _onChallenges += value;
                }
            }
            remove
            {
                lock (_onChallengesLock)
                {
                    _onChallenges -= value;
                }
            }
        }

        private object _onTournamentsLock;
        internal LilaEventHandler<TournamentsEvent> _onTournaments;

        /// <summary>
        /// Occurs when tournaments are updated.
        /// </summary>
        public event LilaEventHandler<TournamentsEvent> OnTournaments
        {
            add
            {
                lock (_onTournamentsLock)
                {
                    _onTournaments += value;
                }
            }
            remove
            {
                lock (_onTournamentsLock)
                {
                    _onTournaments -= value;
                }
            }
        }

        private object _onClientLatencyUpdateLock;
        internal LilaEventHandler<LatencyEvent> _onClientLatencyUpdate;

        /// <summary>
        /// Occurs when client lag/latency is updated.
        /// </summary>
        public event LilaEventHandler<LatencyEvent> OnClientLatencyUpdate
        {
            add
            {
                lock (_onClientLatencyUpdateLock)
                {
                    _onClientLatencyUpdate += value;
                }
            }
            remove
            {
                lock (_onClientLatencyUpdateLock)
                {
                    _onClientLatencyUpdate -= value;
                }
            }
        }

        private object _onJoinGameLock;
        internal LilaEventHandler<JoinGameEvent> _onJoinGame;

        /// <summary>
        /// Occurs when a game is joined.
        /// </summary>
        public event LilaEventHandler<JoinGameEvent> OnJoinGame
        {
            add
            {
                lock (_onJoinGameLock)
                {
                    _onJoinGame += value;
                }
            }
            remove
            {
                lock (_onJoinGameLock)
                {
                    _onJoinGame -= value;
                }
            }
        }

        private object _onTournamentEnterLock;
        internal LilaEventHandler<TournamentEvent> _onTournamentEnter;

        /// <summary>
        /// Occurs when client enters a tournament.
        /// </summary>
        public event LilaEventHandler<TournamentEvent> OnTournamentEnter
        {
            add
            {
                lock (_onTournamentEnterLock)
                {
                    _onTournamentEnter += value;
                }
            }
            remove
            {
                lock (_onTournamentEnterLock)
                {
                    _onTournamentEnter -= value;
                }
            }
        }


        private object _onTournamentJoinLock;
        internal LilaEventHandler<TournamentEvent> _onTournamentJoin;

        /// <summary>
        /// Occurs when client joins/starts playing a tournament
        /// </summary>
        public event LilaEventHandler<TournamentEvent> OnTournamentJoin
        {
            add
            {
                lock (_onTournamentJoinLock)
                {
                    _onTournamentJoin += value;
                }
            }
            remove
            {
                lock (_onTournamentJoinLock)
                {
                    _onTournamentJoin -= value;
                }
            }
        }

        /// <summary>
        /// Fires the event asynchronously.
        /// </summary>
        /// <param name="d">The delegate to fire.</param>
        /// <param name="e">The event args to fire with.</param>
        internal void FireEventAsync(Delegate d, LilaEvent e)
        {
            if (d != null && e != null)
            {
                Task t = Task.Run(() => d.DynamicInvoke(this, e));
                t.ContinueWith(DisposeTask);
            }
        }

        /// <summary>
        /// Callback for disposing a task.
        /// </summary>
        /// <param name="obj">The task.</param>
        private void DisposeTask(Task obj)
        {
            obj.Dispose();
        }

        /// <summary>
        /// Fires the event synchronously. 
        /// </summary>
        /// <param name="d">The delegate to fire.</param>
        /// <param name="e">The event args to fire with.</param>
        /// <returns>True if delegate is not null; otherwise False</returns>
        internal bool FireEvent(Delegate d, LilaEvent e)
        {
            if (d != null)
            {
                d.DynamicInvoke(this, e);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LilaEvents"/> class.
        /// </summary>
        public LilaEvents()
        {
            _onAuthenticationFailLock = new object();
            _onConnectLock = new object();
            _onDisconnectLock = new object();
            _onNewHookLock = new object();
            _onChallengesLock = new object();
            _onTournamentsLock = new object();
            _onClientLatencyUpdateLock = new object();
            _onJoinGameLock = new object();
            _onTournamentEnterLock = new object();
            _onTournamentJoinLock = new object();
        }
    }
}
