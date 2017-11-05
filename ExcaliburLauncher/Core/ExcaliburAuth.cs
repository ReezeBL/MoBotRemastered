using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExcaliburLauncher.Core
{
    internal static class ExcaliburAuth
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        private const string LoginUrl = "http://ex-server.ru/exAuthLogin.php";
        private const string ConfigUrl = "http://ex-server.ru/configlauncher.xml";

        public static async Task<AuthData> GetAuthSession(string username, string password)
        {
            var authParams = new Dictionary<string, string>
            {
                ["user"] = username,
                ["pass"] = password,
                ["guid"] = Guid.NewGuid().ToString().Replace("-", ""),
            };
            var content = new FormUrlEncodedContent(authParams);

            var response = await HttpClient.PostAsync(LoginUrl, content);
            if(response.StatusCode != HttpStatusCode.OK)
                throw new Exception("Failed to load auth data!");

            var xml = XDocument.Parse(await response.Content.ReadAsStringAsync());
            var authElement = xml.Element("ex-auth");
            if (authElement?.Element("error")?.Value != "false")
                throw new Exception(authElement?.Element("error-mess")?.Value);
            return new AuthData(authElement.Element("session")?.Value, authElement.Element("uuid")?.Value, username);
        }

        public static async Task<List<ServerConfig>> GetConfig()
        {
            var response = await HttpClient.GetAsync(ConfigUrl);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception("Failed to load config data!");

            var content = await response.Content.ReadAsStringAsync();
            var xml = XDocument.Parse(content);
            return xml.Element("ex-servers")?.Elements("server").Select(xelement => new ServerConfig(xelement)).ToList();
        }

        public class AuthData
        {
            public AuthData(string session, string uuid, string username)
            {
                Session = session;
                Uuid = uuid;
                Username = username;
            }

            public string Username { get; }
            public string Session { get; }
            public string Uuid { get; }
        }

        public class ServerConfig
        {
            public ServerConfig(XContainer node)
            {
                Name = node.Element("name")?.Value;

                var appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                Directory = Path.Combine(appdataPath, ".exclient", node.Element("dir")?.Value ?? string.Empty);
                
                Version = node.Element("version")?.Value;

                var paths = node.Element("class-path")?.Elements("path").Select(element => element?.Value);
                if(paths != null)
                    ClassPath.AddRange(paths);

                MainClass = node.Element("main-class")?.Value;
                ExtraArguments = node.Element("extra-arguments")?.Value;
                Arguments = node.Element("arguments")?.Value;
            }

            public string Name { get; }
            public string Directory { get; set; }
            public string Version { get; }
            public List<string> ClassPath { get; } = new List<string>();
            public string MainClass { get; }
            public string ExtraArguments { get; }
            public string Arguments { get; }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
