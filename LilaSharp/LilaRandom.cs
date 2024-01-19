using NLog;
using System;
using System.Security.Cryptography;

namespace LilaSharp
{
    /// <summary>
    /// Secure random class implementing extensions of basic random functions.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class LilaRandom : IDisposable
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        private const string Base36 = "0123456789abcdefghijklmnopqrstuvwxyz‏";

        private RNGCryptoServiceProvider crypto;

        /// <summary>
        /// Generates the next secure pseudo-random number.
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">minValue</exception>
        public int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException("minValue");
            }

            if (minValue == maxValue)
            {
                return minValue;
            }

            byte[] buffer = new byte[4];
            long diff = maxValue - minValue;

            uint rand;
            long max = (1 + (long)uint.MaxValue);

            do
            {
                crypto.GetBytes(buffer);
            }
            while ((rand = BitConverter.ToUInt32(buffer, 0)) >= max - (max % diff));
            return (int)(minValue + (rand % diff));
        }

        /// <summary>
        /// Generates the next sri string value.
        /// </summary>
        /// <returns></returns>
        public string NextSri()
        {
            int len = Next(10, 12);
            return Create(len);
        }

        /// <summary>
        /// Creates a string of the specified size.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>Random alphanumeric string</returns>
        private string Create(int size)
        {
            char[] buffer = new char[size];
            for (var cnt = 0; cnt < size; cnt++)
            {
                buffer[cnt] = Base36[Next(0, 36)];
            }

            return new string(buffer);
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
            System.Diagnostics.Debug.WriteLine("~LilaRandom");
            if (crypto != null)
            {
                crypto.Dispose();
                crypto = null;
            }

            if (finalize)
            {
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LilaRandom"/> class.
        /// </summary>
        public LilaRandom()
        {
            crypto = new RNGCryptoServiceProvider();
        }
    }
}
