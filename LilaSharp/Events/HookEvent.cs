using LilaSharp.Types;

namespace LilaSharp.Events
{
    public class HookEvent : LilaEvent
    {
        /// <summary>
        /// Gets or sets the hook.
        /// </summary>
        /// <value>
        /// The hook.
        /// </value>
        public IHook Hook { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HookEvent"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="hook">The hook.</param>
        public HookEvent(LilaClient client, IHook hook) : base(client)
        {
            Hook = hook;
        }
    }
}
