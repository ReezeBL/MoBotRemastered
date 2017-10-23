using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Handshake
{
    internal class EmptyPacket : Packet
    {
        public override void HandlePacket(IHandler handler)
        {
            
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            
        }
    }
}
