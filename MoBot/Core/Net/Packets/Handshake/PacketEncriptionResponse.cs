using System;
using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Handshake
{
    internal class PacketEncriptionResponse : Packet
    {
        public int SharedSecretLength = 0;
        public byte[] SharedSecret = null;
        public int TokenLength = 0;
        public byte[] Token = null;
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
            buff.WriteShort((short)SharedSecretLength);
            buff.WriteBytes(SharedSecret);
            buff.WriteShort((short)TokenLength);
            buff.WriteBytes(Token);
        }
    }
}
