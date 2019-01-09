using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Play
{
    internal class PacketHeldItemChange : Packet
    {
        public byte Slot;
        public virtual void HandlePacket(IHandler handler)
        {
            handler.HandlePacketHeldItemChange(this);
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            Slot = buff.ReadByte();
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            buff.WriteShort(Slot);
        }
    }
}
