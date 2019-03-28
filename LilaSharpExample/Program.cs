using LilaSharp;
using LilaSharp.Events;
using System;

namespace LilaSharpExample
{
    class Program
    {
        static void Main(string[] args)
        {
            using (LilaClient client = new LilaClient())
            {
                client.Events.OnNewHook += OnNewHook;

                //Login as anonymous
                client.Login();

                Console.WriteLine("Ready.");
                Console.ReadKey();
            }
        }

        private static void OnNewHook(object sender, HookEvent e)
        {
            Console.WriteLine("Hook received {0}", e.Hook.Rating);
        }

        private static void OnChallenges(object sender, ChallengesEvent e)
        {
            
        }
    }
}
