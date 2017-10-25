namespace MoBot.Core.Net.Handlers
{
    public interface IAuthHandler
    {
        bool HandleAuth(string username, string password, string serverId);
    }
}