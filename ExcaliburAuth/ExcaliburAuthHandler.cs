using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Xml.Linq;
using MoBot.Core.Net.Handlers;
using MoBot.Core.Plugins;

namespace ExcaliburAuth
{
    public class ExcaliburAuthHandler : IPlugin, IAuthHandler
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public string Name => "Excalibut Auth Handler";
        public string Author => "Siamant";
        public string Version => "0.0.1";

        public void Initialize()
        {
            StaticContext.AuthHandler = this;
        }

        public void HandleAuth(string username, string password, string serverId)
        {
            var session = GetAuthSession(username, password);
            var authParams = new Dictionary<string, string>
            {
                ["user"] = username,
                ["sessionId"] = session,
                ["serverId"] = serverId,
            };
            var content = new FormUrlEncodedContent(authParams);
            var response = httpClient.PostAsync("http://ex-server.ru/joinserver.php", content).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
        }

        public string GetAuthSession(string username, string password)
        {
            var authParams = new Dictionary<string, string>
            {
                ["user"]=username,
                ["pass"]=password,
                ["guid"]=Guid.NewGuid().ToString().Replace("-", ""),
            };
            var content = new FormUrlEncodedContent(authParams);

            var response = httpClient.PostAsync("http://ex-server.ru/exAuthLogin.php", content).Result;

            var xml = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
            var authElement = xml.Element("ex-auth");
            if (authElement?.Element("error")?.Value != "false")
                throw new UnauthorizedAccessException(authElement?.Element("error-mess")?.Value);
            return authElement.Element("session")?.Value;
        }

    }
}
