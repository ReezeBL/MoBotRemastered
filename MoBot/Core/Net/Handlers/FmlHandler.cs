using MoBot.Core.Net.Packets.Play;
using NLog;

namespace MoBot.Core.Net.Handlers
{
    internal class FmlHandler : ICustomHandler
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly MoBase baseInstance;

        public FmlHandler(MoBase baseInstance)
        {
            this.baseInstance = baseInstance;
        }

        public void ProcessPacketData(byte[] data)
        {
            var payload = new StreamWrapper(data);
            var discriminator = payload.ReadByte();
            switch (discriminator)
            {
                case 0:
                {
                    var answer = new StreamWrapper();
                    var version = payload.ReadByte();
                    answer.WriteByte(1);
                    answer.WriteByte(version);
                    baseInstance.ConnectionChannel.SendPacket(new PacketCustomPayload
                    {
                        Channel = "FML|HS",
                        Payload = answer.GetBlob()
                    });

                    answer = new StreamWrapper();
                    answer.WriteByte(2);
                    answer.WriteVarInt(baseInstance.ModList.Length);
                    foreach (var modInfo in baseInstance.ModList)
                    {
                        answer.WriteString(modInfo.modid);
                        answer.WriteString(modInfo.version);
                    }
                    baseInstance.ConnectionChannel.SendPacket(new PacketCustomPayload
                    {
                        Channel = "FML|HS",
                        Payload = answer.GetBlob()
                    });
                }
                    break;
                case 2:
                {
                    var answer = new StreamWrapper();
                    answer.WriteByte(255);
                    answer.WriteByte(2);
                    baseInstance.ConnectionChannel.SendPacket(new PacketCustomPayload
                    {
                        Channel = "FML|HS",
                        Payload = answer.GetBlob()
                    });
                }
                    break;
                case 255:
                {
                    var stage = payload.ReadByte();
                    var answer = new StreamWrapper();
                    answer.WriteByte(255);
                    switch (stage)
                    {
                        case 3:
                            answer.WriteByte(5);
                            break;
                        case 2:
                            answer.WriteByte(4);
                            break;
                        default:
                            Logger.Info($"Unhandled Ack Stage : {stage}");
                            break;
                    }
                    baseInstance.ConnectionChannel.SendPacket(new PacketCustomPayload
                    {
                        Channel = "FML|HS",
                        Payload = answer.GetBlob()
                    });
                }
                    break;
            }
        }
    }
}
