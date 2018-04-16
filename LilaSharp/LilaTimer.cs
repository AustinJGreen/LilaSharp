using LilaSharp.Delegates;
using NLog;
using System;
using System.Diagnostics;
using System.Threading;

namespace LilaSharp
{
    /// <summary>
    /// Timer implementation for handling callbacks and scheduling
    /// </summary>
    /// <typeparam name="T">Type used in callback. <see cref="LilaSharp.Delegates.LilaCallback{T}"/></typeparam>
    /// <seealso cref="LilaSharp.LilaDebug" />
    /// <seealso cref="System.IDisposable" />
    internal class LilaTimer<T> : LilaDebug, IDisposable
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        private object timerLock;

        private Timer timer;
        private LilaCallback<T> callback;

        private bool running;
        private Stopwatch sw;

        /// <summary>
        /// Gets or sets the period between ticks in milliseconds.
        /// </summary>
        /// <value>
        /// The period.
        /// </value>
        public int Period { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public T Data { get; set; }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            if (running)
            {
                return;
            }

            lock (timerLock)
            {
                if (timer != null)
                {
                    timer.Change(Period, -1);
                    sw.Restart();
                    running = true;
                }
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (!running)
            {
                return;
            }

            lock (timerLock)
            {
                if (timer != null)
                {
                    timer.Change(-1, Period);
                    sw.Reset();
                    running = false;
                }
            }
        }

        /// <summary>
        /// Called when the timer ticks.
        /// </summary>
        /// <param name="state">The state.</param>
        private void OnInternalCallback(object state)
        {
            sw.Stop();
            if (!running)
            {
                return;
            }

            long offset = 0;
            if ((offset = (sw.ElapsedMilliseconds - Period)) > 50)
            {
                log.ConditionalDebug("Timer was {0}ms off schedule.", offset);
            }

            if (callback != null)
            {
                try
                {
                    callback.Invoke(Data);
                }
                catch (Exception ex)
                {
                    log.Error(ex, "Error in timer callback.");
                }
            }

            lock (timerLock)
            {
                if (timer != null)
                {
                    timer.Change(Period, -1);
                }
            }
            sw.Restart();
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
            log.ConditionalTrace("~LilaTimer");

            running = false;

            lock (timerLock)
            {         
                if (timer != null)
                {
                    timer.Dispose();
                    timer = null;
                }
            }

            if (finalize)
            {
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LilaTimer{T}"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="period">The period between ticks in milliseconds.</param>
        /// <param name="data">The data.</param>
        /// <exception cref="ArgumentNullException">callback - Callback cannot be null</exception>
        public LilaTimer(LilaCallback<T> callback, int period, T data)
        {
            this.callback = callback ?? throw new ArgumentNullException("callback", "Callback cannot be null");

            timerLock = new object();

            timer = new Timer(OnInternalCallback);
            timer.Change(-1, -1);

            Period = period;
            Data = data;

            sw = new Stopwatch();
            running = false;
        }
    }
}
