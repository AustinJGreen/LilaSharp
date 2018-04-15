using LilaSharp.Messages;

namespace LilaSharp.Internal
{
    /// <summary>
    /// Handler for message callbacks parsed from sockets.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ws">The websocket.</param>
    /// <param name="message">The message.</param>
    internal delegate void MessageHandler<T>(WebSocketBase ws, T message) where T : IMessage;
} 
