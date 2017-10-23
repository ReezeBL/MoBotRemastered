using System.IO;
using System.Net;

namespace MoBot.Helpers
{
    internal static class HttpHelper
    {
        public static string PostUrl(string username, string sessionId, string serverId)
        {
            var request = WebRequest.Create($"http://ex-server.ru/joinserver.php?user={username}&sessionId={sessionId}&serverId={serverId}");
            var responseStream = request.GetResponse().GetResponseStream();
            if (responseStream == null) return "";
            var responseString = new StreamReader(responseStream).ReadToEnd();
            return responseString;
        }

    }
}
