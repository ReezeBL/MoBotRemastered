using System;
using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Play
{
    internal class PacketPlayerDigging : Packet
    {
        public byte Status;
        public int X;
        public byte Y;
        public int Z;
        public byte Face;

        public override void HandlePacket(IHandler handler)
        {
            throw new NotImplementedException();
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            throw new NotImplementedException();
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            buff.WriteByte(Status);
            buff.WriteInt(X);
            buff.WriteByte(Y);
            buff.WriteInt(Z);
            buff.WriteByte(Face);
        }
    }
}
