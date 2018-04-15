using LilaSharp.Events;

namespace LilaSharp.Delegates
{
    /// <summary>
    /// Event handler for lichess events
    /// </summary>
    /// <typeparam name="E"></typeparam>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event data.</param>
    public delegate void LilaEventHandler<E>(object sender, E e) where E : LilaEvent;
}
