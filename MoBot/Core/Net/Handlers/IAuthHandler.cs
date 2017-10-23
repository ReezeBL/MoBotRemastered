namespace MoBot.Core.Net.Handlers
{
    public interface IAuthHandler
    {
        void HandleAuth(string username, string password, string serverId);
    }
}