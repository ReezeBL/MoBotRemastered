using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Handshake
{
    internal class PacketLoginSuccess : Packet
    {
        public virtual void HandlePacket(IHandler handler)
        {
            handler.HandlePacketLoginSucess(this);
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            throw new System.NotImplementedException();
        }

        public override bool ProceedNow()
        {
            return true;
        }
    }
}