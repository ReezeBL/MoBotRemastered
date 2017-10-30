using System.Collections.Generic;
using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Plugins
{
    public static class GlobalModules
    {
        private static IAuthHandler authHandler;
        private static readonly Dictionary<string, MoBase> InstancesInWork = new Dictionary<string, MoBase>();

        public static IAuthHandler AuthHandler
        {
            get => authHandler ?? (authHandler = new DefaultAuthHandler());
            set => authHandler = value;
        }

        public static bool IsProfileLoaded(string profile) => InstancesInWork.ContainsKey(profile);
        public static void AddProfile(string profile, MoBase instance) => InstancesInWork.Add(profile, instance);
        public static void UnloadProfile(string profile) => InstancesInWork.Remove(profile);

    }
}