using System;
using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Play
{
    internal class PacketDestroyEntities : Packet
    {
        public byte Length;
        public int[] IdList;
        public override void HandlePacket(IHandler handler)
        {
            handler.HandlePacketDestroyEntities(this);
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            Length = buff.ReadByte();
            IdList = new int[Length];
            for (var i = 0; i < Length; i++)
                IdList[i] = buff.ReadInt();
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            throw new NotImplementedException();
        }
    }
}
