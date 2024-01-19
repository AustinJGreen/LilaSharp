using LilaSharp;
using LilaSharp.API;
using LilaSharp.Events;
using System;
using System.Diagnostics;

namespace LilaSharpExample
{
    class Program
    {
        static bool gameJoinRequestMade = false;
        static void Main(string[] args)
        {
            using (LilaClient client = new LilaClient())
            {
                client.Events.OnConnect += OnConnect;
                client.Events.OnNewHook += OnNewHook;
                client.Events.OnJoinGame += OnJoinGame;
                client.Events.OnChallenges += OnChallenges;

                //client.Login("enishyseni@gmail.com", "1a2b3c4d5e");
                //Login as anonymous
                client.Login();
                Console.WriteLine("Ready.");
                Console.Read();
            }
        }

        private static async void OnNewHook(object sender, HookEvent e)
        {
            // if (e.Hook.Rating > 0)
            // {
            //     Console.WriteLine("Hook received: {0}", e.Hook.Rating);
            // }

            //Console.WriteLine("Games active: {0}", e.Client.Games);
            //Console.WriteLine("Game connections: {0}", e.Client.GameConnections);

            // try
            // {
            //     if (e.Client.Hooks.Count() > 0)
            //     {
            //         foreach (var a in e.Client.Hooks.ToList())
            //         {
            //             if (!gameJoinRequestMade) // && a.Username != null)
            //             {
            //                 gameJoinRequestMade = true;
            //                 Debug.WriteLine("Hook username: {0}", a.Username);
            //                 e.Client.JoinGame(a);
            //             }
            //         }
            //     }
            // }
            // catch { }

            // foreach (var a in e.Client.Hooks.ToList())
            // {
            //     Console.WriteLine("Hook username: {0}", a.Username);
            //     await e.Client.ChallengePlayer(a.Username, new GameForm()
            //     {
            //         Color = "black",
            //         Days = 0,
            //         Fen = "",
            //         Increment = 1,
            //         Mode = 1,
            //         Time = 60,
            //         TimeMode = 1,
            //         Variant = 1
            //     });
            //     await Task.Delay(5000);
            // }
        }

        private static void OnJoinGame(object sender, JoinGameEvent e)
        {
            Console.WriteLine("Game joined. Opponent: {0}", e.Game.Data.Opponent.Color);
            e.Game.OnGameMove += OnGameMove;
            e.Game.OnGameEnd += OnGameEnd;
        }

        private static async void OnConnect(object sender, ConnectEvent e)
        {
            Console.WriteLine("Connected: {0}", e.Client.LobbySri);

            //e.Client.WatchGame("BbLQru2Y5VdF");
        }

        private static void OnChallenges(object sender, ChallengesEvent e)
        {
            Console.WriteLine("Challanges: {0}", e.Challenges.Out);
        }

        private static void OnGameMove(object sender, LilaGameMoveEvent e)
        {
            Console.WriteLine("Move: {0}", e.Move.Uci);
            // if (e.Game.Data.Player.Color == "white")
            // {
            //     e.Game.PlayMove("h2h4");
            // }
            // if (e.Game.Data.Player.Color == "black")
            // {
            //     e.Game.PlayMove("a7a5");
            // }
        }

        private static void OnGameEnd(object sender, LilaGameEvent e)
        {
            Console.WriteLine("Game ended ID: {0}", e.Game.Data.Board.Id);
        }
    }
}
