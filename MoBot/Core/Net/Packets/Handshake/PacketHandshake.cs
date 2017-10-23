using System;
using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Handshake
{
    internal class PacketHandshake : Packet
    {
        public int ProtocolVersion;
        public string Hostname;
        public ushort Port;
        public int NextState;

        public override void HandlePacket(IHandler handler)
        {
            throw new NotImplementedException();
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            ProtocolVersion = buff.ReadVarInt();
            Hostname = buff.ReadString();
            Port = (ushort)buff.ReadShort();
            NextState = buff.ReadVarInt();
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            buff.WriteVarInt(ProtocolVersion);
            buff.WriteString(Hostname);
            buff.WriteShort((short)Port);
            buff.WriteVarInt(NextState);
        }
    }
}
