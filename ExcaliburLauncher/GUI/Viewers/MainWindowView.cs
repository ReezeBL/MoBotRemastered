using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ExcaliburLauncher.Core;
using ExcaliburLauncher.GUI.Commands;
using Newtonsoft.Json;

namespace ExcaliburLauncher.GUI.Viewers
{
    internal class MainWindowView : AbsractView
    {
        private const string ConfigsPath = "configs.json";
        private const string SettingsPath = "user.json";
        private static readonly Task<List<ExcaliburAuth.ServerConfig>> GetConfigsTask = ExcaliburAuth.GetConfig();

        public MainWindowView()
        {
            SaveCommand = new RelayCommand(SaveData);
            ConnectCommand = new AsyncCommand(Connect);
        }

        #region Bindings

        private List<ServerConfigView> serverConfigViews;
        private ServerConfigView selectedConfig;
        public List<ServerConfigView> ServerConfigViews => serverConfigViews ?? (serverConfigViews = CreateServerConfigs());
        public string JavaPath { get; set; } = GetDefaultJavaPath();
        public ServerConfigView SelectedConfig
        {
            get => selectedConfig;
            set
            {
                if (Equals(value, selectedConfig)) return;
                selectedConfig = value;
                OnPropertyChanged();
            }
        }
        public UserSettingsView UserSettings { get; set; } = GetUserSettings();

        public ICommand SaveCommand { get; set; }
        public ICommand ConnectCommand { get; set; }

        #endregion

        private void SaveData(object context)
        {
            var serverConfigs = serverConfigViews.Select(view => view.ServerConfig).ToArray();
            SaveConfigsToFile(serverConfigs);
            SaveUserSettings(UserSettings);
        }

        private async Task Connect()
        {
            var authData = await ExcaliburAuth.GetAuthSession(UserSettings.Username, UserSettings.Password);
            MinecraftLoader.StartJavaProcess(JavaPath, authData, selectedConfig.ServerConfig);
        }

        #region Static Functions

        private static List<ServerConfigView> CreateServerConfigs()
        {
            var serverConfigs =
                LoadConfigsFromFile() ?? GetConfigsTask.Result ?? new List<ExcaliburAuth.ServerConfig>();
            return serverConfigs.Select(config => new ServerConfigView(config)).ToList();
        }

        private static UserSettingsView GetUserSettings()
        {
            if (!File.Exists(SettingsPath))
                return new UserSettingsView();

            using (var file = File.OpenText(SettingsPath))
            using (var reader = new JsonTextReader(file))
            {
                var deserializer = new JsonSerializer();
                return deserializer.Deserialize<UserSettingsView>(reader);
            }
        }

        private static void SaveUserSettings(UserSettingsView settings)
        {
            using (var file = File.CreateText(SettingsPath))
            using (var writer = new JsonTextWriter(file))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, settings);
            }
        }

        private static string GetDefaultJavaPath()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = Path.Combine(path, "exjava", "jvm", "bin");
            return path;
        }

        private static List<ExcaliburAuth.ServerConfig> LoadConfigsFromFile()
        {
            if (!File.Exists(ConfigsPath))
                return null;

            using (var file = File.OpenText(ConfigsPath))
            using (var reader = new JsonTextReader(file))
            {
                var deserializer = new JsonSerializer();
                return deserializer.Deserialize<List<ExcaliburAuth.ServerConfig>>(reader);
            }
        }

        private static void SaveConfigsToFile(IReadOnlyCollection<ExcaliburAuth.ServerConfig> configs)
        {
            using (var file = File.CreateText(ConfigsPath))
            using (var writer = new JsonTextWriter(file))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, configs);
            }
        }

        #endregion
    }
}
