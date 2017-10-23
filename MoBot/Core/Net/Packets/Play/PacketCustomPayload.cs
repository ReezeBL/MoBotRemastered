using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Play
{
    internal class PacketCustomPayload : Packet
    {
        public string Channel;
        public byte[] Payload;

        public override void HandlePacket(IHandler handler)
        {
            handler.HandlePacketCustomPayload(this);
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            Channel = buff.ReadString();
            var length = (ushort)buff.ReadShort();
            Payload = buff.ReadBytes(length);
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            buff.WriteString(Channel);
            buff.WriteShort((short)Payload.Length);
            buff.WriteBytes(Payload);
        }
    }
}
