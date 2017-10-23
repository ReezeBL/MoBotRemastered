using System;
using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Handshake
{
    internal class PacketLoginStart : Packet
    {
        public string Name = "";
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
            buff.WriteString(Name);
        }
    }
}
