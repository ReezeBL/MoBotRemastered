using System;
using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Play
{
    internal class PacketSpawnMob : Packet
    {
        public int EntityId;
        public byte Type;
        public double X, Y, Z;

        public virtual void HandlePacket(IHandler handler)
        {
            handler.HandlePacketSpawnMoob(this);
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            EntityId = buff.ReadVarInt();
            Type = buff.ReadByte();
            X = buff.ReadInt() / 32.0;
            Y = buff.ReadInt() / 32.0;
            Z = buff.ReadInt() / 32.0;
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            throw new NotImplementedException();
        }
    }
}
