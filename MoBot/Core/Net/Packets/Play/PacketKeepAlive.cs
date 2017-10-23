using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Play
{
    internal class PacketKeepAlive : Packet
    {
        public int Seed;
        public override void HandlePacket(IHandler handler)
        {
            handler.HandlePacketKeepAlive(this);
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            Seed = buff.ReadInt();
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            buff.WriteInt(Seed);
        }

        public override bool ProceedNow()
        {
            return true;
        }
    }
}
