using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using MoBot.Core.Net;
using MoBot.Core.Net.Handlers;
using MoBot.Core.Net.Packets.Handshake;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace MoBot.Core
{
    public class MoBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public MoBase(string profile)
        {
            PacketHandler = new ClientHandler(this);
            PacketHandler.RegisterCustomHandler("FML|HS", new FmlHandler(this));

            UserSettings = Settings.LoadProfile(profile);
            Settings.SyncProfile(profile, UserSettings);
        }

        internal NetworkManager NetworkManager { get; private set; }
        internal Settings.UserSettings UserSettings { get; }
        internal IHandler PacketHandler { get; }
        internal ModInfo[] ModList { get; } = GetModList();

        public GameController GameController { get; private set; }
        public bool Connected => NetworkManager != null && NetworkManager.IsRunning;

        public event EventHandler<string> Notify;

        public async Task<bool> Connect()
        {
            if (Connected)
                return false;

            var response = await Ping(UserSettings.ServerIp, UserSettings.ServerPort);
            if (response == null)
            {
                OnNotify("Server is unavailble");
                return false;
            }
            if (response.players.online >= response.players.max)
            {
                OnNotify("Server is full");
                return false;
            }

            GameController = new GameController();

            var client = new TcpClient();
            await client.ConnectAsync(UserSettings.ServerIp, UserSettings.ServerPort);

            NetworkManager = new NetworkManager(PacketHandler, client.GetStream());
            NetworkManager.SetupThreads();

            NetworkManager.AddToSendingQueue(new PacketHandshake
            {
                Hostname = UserSettings.ServerIp,
                Port = (ushort) UserSettings.ServerPort,
                NextState = 2,
                ProtocolVersion = (int) response.version.protocol
            });
            NetworkManager.AddToSendingQueue(new PacketLoginStart {Name = UserSettings.Username});

            
            return true;
        }

        public async Task<dynamic> Ping(string ip, int port)
        {
            var client = new TcpClient();
            var connection = client.ConnectAsync(ip, port);
            if (await Task.WhenAny(connection, Task.Delay(2000)) != connection) return null;
            if (!client.Connected)
                return null;
            try
            {
                var pingChannel = new Channel(client.GetStream());
                pingChannel.SendPacket(new PacketHandshake
                {
                    Hostname = ip,
                    Port = (ushort) port,
                    NextState = 1,
                    ProtocolVersion = 46
                });
                pingChannel.SendPacket(new EmptyPacket());

                if (!(pingChannel.GetPacket() is PacketResponse result))
                    return null;

                dynamic response = JObject.Parse(result.JsonResponse);
                client.Close();
                return response;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public void Disconnect()
        {
            NetworkManager.StopThreads();
        }

        public virtual void OnNotify(string e)
        {
            Notify?.Invoke(this, e);
        }

        private static ModInfo[] GetModList()
        {
            ModInfo[] result;
            using (var file = File.OpenText(Settings.ModsPath))
            using (var reader = new JsonTextReader(file))
            {
                var deserializer = JsonSerializer.Create();
                result = deserializer.Deserialize<ModInfo[]>(reader);
            }
            return result;
        }

        public class ModInfo
        {
            public string modid;
            public string version;
        }
    }
}