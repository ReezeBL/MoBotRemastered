using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NLog;

namespace MoBot
{
    [Serializable]
    public class Settings
    {
        private const string Path = "Settings/Settings.json";
        public const string BlocksPath = "Settings/blocks.json";
        public const string ItemsPath = "Settings/items.json";
        public const string EntitiesPath = "Settings/entities.json";
        public const string ModsPath = "Settings/mods.json";
        public const string ScriptsPath = "Scripts";
        public const string MaterialsPath = "Settings/materials.json";
        public const string UserIdsPath = "Settings/UserIDS.xml";

        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        [JsonRequired] private readonly Dictionary<string, UserSettings> userSettingses =
            new Dictionary<string, UserSettings> {{"Default", new UserSettings()}};
        [JsonRequired] private readonly Dictionary<string, string> compiledModules = new Dictionary<string, string>();

        private Settings() { }

        public static string[] GetProfiles()
        {
            var settingsInstance = Deserialize();
            return settingsInstance.userSettingses.Keys.ToArray();
        }

        public static UserSettings LoadProfile(string profile)
        {
            var settingsInstance = Deserialize();
            return settingsInstance.userSettingses.TryGetValue(profile, out UserSettings result)
                ? result
                : new UserSettings();
        }

        public static void SyncProfile(string profile, UserSettings settings)
        {
            var settingsInstance = Deserialize();
            settingsInstance.userSettingses[profile] = settings;
            Serialize(settingsInstance);
        }

        public IReadOnlyDictionary<string, string> GetCompiledModules()
        {
            var settingsInstance = Deserialize();
            return settingsInstance.compiledModules;
        }

        private static Settings Deserialize()
        {
            var serializer = JsonSerializer.CreateDefault();
            try
            {
                using (TextReader stream = new StreamReader(Path))
                using (var reader = new JsonTextReader(stream))
                {
                    return serializer.Deserialize<Settings>(reader);
                }
            }
            catch (Exception)
            {
                Logger.Warn("There is no settings file. Created a new one");
                var settingsInstance = new Settings();
                Serialize(settingsInstance);
                return settingsInstance;
            }
        }

        private static void Serialize(Settings settings)
        {
            var serializer = JsonSerializer.Create(new JsonSerializerSettings { Formatting = Formatting.Indented });
            using (TextWriter stream = new StreamWriter(Path))
            using (var writer = new JsonTextWriter(stream))
            {
                serializer.Serialize(writer, settings);
            }
        }

        [JsonObject(MemberSerialization.Fields)]
        public class UserSettings
        {
            public bool AutoReconnect;

            public string BackWarp = "";
            public string HomeWarp = "";

            public HashSet<int> IntrestedBlocks = new HashSet<int>();
            public HashSet<int> KeepItemIds = new HashSet<int>();
            public int ScanRange = 32;

            public string ServerIp = "";
            public int ServerPort = 1488;

            public string Username = "";
            public string UserToken = "";
        }
    }
}
