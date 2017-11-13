namespace MoBot.GUI.View
{
    internal class UserSettingsView  : AbsractView
    {
        public Settings.UserSettings Settings { get; private set; }
        public string Profile { get; }

        public UserSettingsView(string profile)
        {
            Profile = profile;
        }

        public void Select()
        {
            Settings = MoBot.Settings.LoadProfile(Profile);
        }

        public string Username
        {
            get => Settings.Username;
            set => Settings.Username = value;
        }

        public string Password
        {
            get => Settings.UserToken;
            set => Settings.UserToken = value;
        }

        public string ServerIp
        {
            get => Settings.ServerIp;
            set => Settings.ServerIp = value;
        }

        public int ServerPort
        {
            get => Settings.ServerPort;
            set => Settings.ServerPort = value;
        }

        public void SaveProfile()
        {
            MoBot.Settings.SyncProfile(Profile, Settings);
        }
    }
}
