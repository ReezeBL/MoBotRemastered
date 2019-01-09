using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using MoBot.Core.Net.Handlers;
using NLog;

namespace MoBot.Core.Net
{
    internal class NetworkManager
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        
        private readonly Channel connectionChannel;
        private readonly ClientHandler handler;

        public NetworkManager(Channel connectionChannel, ClientHandler handler)
        {
            this.connectionChannel = connectionChannel;
            this.handler = handler;
        }

        public void Connect(string ip, int port)
        {
            var client = new TcpClient();
            client.ConnectAsync(ip, port).ContinueWith(OnConnectionComplete, client).Start();
        }

        private void OnConnectionComplete(Task connectionTask, object state)
        {
            var client = (TcpClient) state;
            if (!client.Connected)
            {
                Logger.Error("Failed to connect to server!");
                return;
            }

            connectionChannel.SetSource(client);

            Task.Run((Action) ReadPacket);
        }

        private void ReadPacket()
        {
            if(!connectionChannel.IsAlive)
                return;
            
            var packet = connectionChannel.GetPacket();
            if (packet != null)
            {
                if(packet.ProceedNow())
                    handler.ProcessPacket(packet);
                else
                    Task.Run(() => handler.ProcessPacket(packet));
            }
            Task.Run((Action)ReadPacket);
        }
        
    }
}
