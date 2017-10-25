using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Plugins
{
    public static class GlobalModules
    {
        private static IAuthHandler authHandler;

        public static IAuthHandler AuthHandler
        {
            get => authHandler ?? (authHandler = new DefaultAuthHandler());
            set => authHandler = value;
        }
    }
}