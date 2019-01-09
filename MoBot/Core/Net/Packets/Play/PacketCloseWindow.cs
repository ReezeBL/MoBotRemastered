using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Play
{
    internal class PacketCloseWindow : Packet
    {
        public byte WindowId;
        public virtual void HandlePacket(IHandler handler)
        {
            handler.HandlePacketCloseWindow(this);
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            WindowId = buff.ReadByte();
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            buff.WriteByte(WindowId);
        }
    }
}
