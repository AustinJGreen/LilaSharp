using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LilaSharp
{
    /// <summary>
    /// Class used to test best port connection to lichess.
    /// </summary>
    public static class LilaPing
    {
        private static IPAddress[] addresses;

        /// <summary>
        /// Initializes the <see cref="LilaPing"/> class.
        /// </summary>
        static LilaPing()
        {
            Dns.BeginGetHostEntry("socket.lichess.org", OnInitComplete, null);
        }

        /// <summary>
        /// Called when initialize completes.
        /// </summary>
        /// <param name="ar">The ar.</param>
        private static void OnInitComplete(IAsyncResult ar)
        {
            IPHostEntry hostEntry = Dns.EndGetHostEntry(ar);
            addresses = hostEntry.AddressList;
        }

        /// <summary>
        /// Pings the server.
        /// </summary>
        /// <param name="portLow">The port low range.</param>
        /// <param name="portHigh">The port high range.</param>
        /// <param name="epochs">The epochs.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// epochs - Epochs must be greater than 0
        /// or
        /// The range of ports from low to high must be positive
        /// </exception>
        public static int PingServer(int portLow, int portHigh, int epochs)
        {
            if (epochs < 1)
            {
                throw new ArgumentOutOfRangeException("epochs", "Epochs must be greater than 0");
            }
            else if (portHigh - portLow <= 0)
            {
                throw new ArgumentOutOfRangeException("The range of ports from low to high must be positive");
            }

            if (!SpinWait.SpinUntil(() => addresses != null, 2000))
            {
                return -1;
            }

            double[] latencies = new double[portHigh - portLow + 1];
            int[] ports = new int[portHigh - portLow + 1];

            Stopwatch sw = new Stopwatch();
            for (int p = portLow; p <= portHigh; p++)
            {
                ports[p - portLow] = p;

                double total = 0;
                bool failed = false;
                for (int e = 0; e < epochs; e++)
                {               
                    for (int a = 0; a < addresses.Length; a++)
                    {
                        using (TcpClient client = new TcpClient())
                        {
                            client.SendTimeout = 2000;
                            sw.Restart();

                            try
                            {
                                client.Connect(addresses[a], p);
                            }
                            catch
                            {
                                failed = true;
                                break;
                            }

                            sw.Stop();
                            total += sw.ElapsedMilliseconds;
                        }
                    }

                    if (failed)
                    {
                        latencies[p - portLow] = -1;
                        break;
                    }
                }

                if (!failed)
                {
                    latencies[p - portLow] = total / (epochs * addresses.Length);
                }
            }

            Array.Sort(latencies, ports);
            for (int i = 0; i < latencies.Length; i++)
            {
                if (latencies[i] != -1)
                {
                    return ports[i];
                }
            }

            return -1;
        }
    }
}
