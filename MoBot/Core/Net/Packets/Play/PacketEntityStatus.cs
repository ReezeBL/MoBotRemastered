using System;
using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Play
{
    internal class PacketEntityStatus : Packet
    {
        public int EntityId;
        public byte EntityStatus;
        public virtual void HandlePacket(IHandler handler)
        {
            handler.HandlePacketEntityStatus(this);
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            EntityId = buff.ReadInt();
            EntityStatus = buff.ReadByte();
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            throw new NotImplementedException();
        }
    }
}
