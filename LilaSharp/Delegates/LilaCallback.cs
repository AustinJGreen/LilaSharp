namespace LilaSharp.Delegates
{
    /// <summary>
    /// Callback for handling lichess events.
    /// </summary>
    /// <typeparam name="T">The data type.</typeparam>
    /// <param name="data">The data.</param>
    public delegate void LilaCallback<T>(T data);
}
