namespace LilaSharp.Messages
{
    /// <summary>
    /// Lila will restart soon
    /// </summary>
    public class MDeployPre : ITypeMessage
    {
        public string Type => "deployPre";
    }
}
