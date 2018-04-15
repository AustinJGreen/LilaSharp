namespace LilaSharp.Messages
{
    /// <summary>
    /// Lila will restart soon
    /// </summary>
    public class MDeployPre : IMessage
    {
        public string Type => "deployPre";
    }
}
