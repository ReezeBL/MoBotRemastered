using ExcaliburLauncher.Core;

namespace ExcaliburLauncher.GUI.Viewers
{
    public class ServerConfigView : AbsractView
    {
        internal ExcaliburAuth.ServerConfig ServerConfig { get; }

        internal ServerConfigView(ExcaliburAuth.ServerConfig serverConfig)
        {
            ServerConfig = serverConfig;
        }

        public string Directory
        {
            get => ServerConfig.Directory;
            set
            {
                if (value == ServerConfig.Directory) return;
                ServerConfig.Directory = value;
                OnPropertyChanged();
            }
        }

        public string Name => ServerConfig.Name;
    }
}