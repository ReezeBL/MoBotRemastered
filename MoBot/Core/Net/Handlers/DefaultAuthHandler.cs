namespace MoBot.Core.Net.Handlers
{
    internal class DefaultAuthHandler : IAuthHandler
    {
        public bool HandleAuth(string username, string password, string serverId)
        {
            return false;
        }
    }
}
