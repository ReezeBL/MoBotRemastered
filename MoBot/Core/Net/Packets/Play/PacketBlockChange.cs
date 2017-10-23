using System;
using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Play
{
    internal class PacketBlockChange : Packet
    {
        public int X, Z;
        public byte Y;
        public int BlockId;
        public byte BlockMetadata;
        public override void HandlePacket(IHandler handler)
        {
            handler.HandlePacketBlockChange(this);
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            X = buff.ReadInt();
            Y = buff.ReadByte();
            Z = buff.ReadInt();
            BlockId = buff.ReadVarInt();
            BlockMetadata = buff.ReadByte();
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            throw new NotImplementedException();
        }
    }
}
