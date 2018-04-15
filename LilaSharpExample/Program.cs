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
                client.Events.OnChallenges += OnChallenges;

                //Login as anonymous
                client.Login();

                Console.WriteLine("Ready.");
                Console.ReadKey();
            }
        }

        private static void OnChallenges(object sender, ChallengesEvent e)
        {
            Console.WriteLine("Challenges received.");
        }
    }
}
