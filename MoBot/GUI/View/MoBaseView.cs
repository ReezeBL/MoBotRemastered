using System.Threading.Tasks;
using System.Windows.Input;
using MoBot.Core;
using MoBot.Core.Plugins;
using MoBot.GUI.Commands;

namespace MoBot.GUI.View
{
    internal class MoBaseView : AbsractView
    {
        private string profile;
        private MoBase instance;
        private UserSettingsView userSettings;

        public MoBaseView()
        {
            Connection = new ConnectionView(this);
        }

        public MoBase Instance
        {
            get => instance;
            set
            {
                if (Equals(value, instance)) return;
                instance = value;
                OnPropertyChanged();
            }
        }

        public UserSettingsView UserSettings
        {
            get => userSettings;
            set
            {
                if (Equals(value, userSettings)) return;
                userSettings = value;
                OnPropertyChanged();
                Connection.Settings = value;
            }
        }

        public string Profile
        {
            get => profile;
            set
            {
                if (value == profile) return;
                profile = value;
                OnPropertyChanged();

                Instance = GlobalModules.GetProfile(profile);
                UserSettings = new UserSettingsView(profile);
                UserSettings.Select();
            }
        }

        public ConnectionView Connection { get; }

        public class ConnectionView : AbsractView
        {
            private readonly MoBaseView baseView;
            private UserSettingsView settings;
            private bool autoReconnect;
            private bool connected;

            public ConnectionView(MoBaseView baseView)
            {
                this.baseView = baseView;
                Save = new RelayCommand(o => Settings.SaveProfile());
                Connect = new AsyncCommand(ConnectCallback, o => !Connected);
            }

            private async Task ConnectCallback(object context)
            {
                if (context != null && AutoReconnect)
                    await Task.Delay(ReconnectDelay);
                Connected = await baseView.instance.Connect();
            }

            private void OnDisconnect()
            {
                if(AutoReconnect && Connect.CanExecute(null))
                    Connect.Execute(true);
            }

            public UserSettingsView Settings
            {
                get => settings;
                set
                {
                    if (Equals(value, settings)) return;
                    settings = value;
                    OnPropertyChanged();
                }
            }

            public bool Connected
            {
                get => connected;
                set
                {
                    if (value == connected) return;
                    connected = value;
                    OnPropertyChanged();
                }
            }

            public bool AutoReconnect
            {
                get => autoReconnect;
                set
                {
                    if (value == autoReconnect) return;
                    autoReconnect = value;
                    OnPropertyChanged();
                }
            }

            public int ReconnectDelay { get; set; } = 1500;

            public ICommand Save { get; }
            public ICommand Connect { get; }
        }
    }
}
