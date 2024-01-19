using NLog;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace LilaSharp.Internal
{
    /// <summary>
    /// Wrapper around <see cref="System.Threading.Tasks.Task"/>.
    /// </summary>
    internal class WebSocketTask
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        private Task task;
        private TaskStatus result;
        private CancellationTokenSource tokenSource;

        public Task Task => task;

        public event EventHandler OnComplete;

        /// <summary>
        /// Determines whether this instance is success.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is success; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSuccess()
        {
            return (task != null && task.Status == TaskStatus.RanToCompletion) || (result == TaskStatus.RanToCompletion);
        }

        /// <summary>
        /// Handles the completion.
        /// </summary>
        private void HandleCompletion()
        {
            if (task != null && task.IsFaulted)
            {
                for (int i = 0; i < task.Exception.InnerExceptions.Count; i++)
                {
                    System.Diagnostics.Debug.WriteLine(task.Exception.InnerExceptions[i], "WebSocketTask faulted.");
                }
            }

            Dispose();

            OnComplete?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Cancels this task.
        /// </summary>
        public void Cancel()
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel(false);
            }
        }

        /// <summary>
        /// Cancels this task after timespan.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        public void CancelAfter(TimeSpan timeSpan)
        {
            if (tokenSource != null)
            {
                tokenSource.CancelAfter(timeSpan);
            }
        }

        /// <summary>
        /// Waits for the task to complete.
        /// </summary>
        /// <param name="timeout">The timeout to wait for in milliseconds.</param>
        public void Wait(int timeout)
        {
            if (task != null)
            {
                try
                {
                    task.Wait(timeout);
                }
                catch (AggregateException ae)
                {
                    for (int i = 0; i < ae.InnerExceptions.Count; i++)
                    {
                        System.Diagnostics.Debug.WriteLine(ae.InnerExceptions[i], "WebSocketTask faulted.");
                    }
                }
            }
        }

        /// <summary>
        /// Waits the task to complete.s
        /// </summary>
        /// <param name="timeSpan">The time span to wait for.</param>
        public void Wait(TimeSpan timeSpan)
        {
            if (task != null)
            {
                task.Wait(timeSpan);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        private void Dispose()
        {
            if (task != null && task.IsCompleted)
            {
                System.Diagnostics.Debug.WriteLine("~WebSocketTask");
                result = task.Status;
                task.Dispose();
                task = null;
            }

            if (tokenSource != null)
            {
                tokenSource.Dispose();
                tokenSource = null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketTask"/> class.
        /// </summary>
        public WebSocketTask()
        {
            result = TaskStatus.Canceled;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketTask"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="tokenSource">The token source.</param>
        /// <exception cref="ArgumentNullException">
        /// task - task cannot be null.
        /// or
        /// tokenSource - tokenSource cannot be null.
        /// </exception>
        public WebSocketTask(Task task, CancellationTokenSource tokenSource) : this()
        {
            if (task == null)
            {
                throw new ArgumentNullException("task", "task cannot be null.");
            }
            else if (tokenSource == null)
            {
                throw new ArgumentNullException("tokenSource", "tokenSource cannot be null.");
            }

            this.task = task;
            this.tokenSource = tokenSource;

            if (task.IsCanceled || task.IsCompleted || task.IsFaulted)
            {
                Dispose();
            }

            TaskAwaiter tw = task.GetAwaiter();
            tw.OnCompleted(HandleCompletion);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="WebSocketTask"/> class.
        /// </summary>
        ~WebSocketTask()
        {
            Dispose();
        }
    }
}
