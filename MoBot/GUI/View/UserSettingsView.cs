using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MoBot.Annotations;
using MoBot.Core.Plugins;
using MoBot.GUI.Commands;

namespace MoBot.GUI.View
{
    internal class UserSettingsView  : INotifyPropertyChanged
    {
        private readonly Settings.UserSettings settings;
        private readonly string profile;
        private ICommand loadCommand;

        public UserSettingsView(string profile)
        {
            this.profile = profile;
            settings = Settings.LoadProfile(profile);
        }

        public string Username
        {
            get => settings.Username;
            set => settings.Username = value;
        }

        public string ServerIP
        {
            get => settings.ServerIp;
            set => settings.ServerIp = value;
        }

        public int ServerPort
        {
            get => settings.ServerPort;
            set => settings.ServerPort = value;
        }

        public ICommand LoadCommand => loadCommand ?? (loadCommand = new LoadProfile(profile));

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SaveProfile()
        {
            Settings.SyncProfile(profile, settings);
        }
    }
}
