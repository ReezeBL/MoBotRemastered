using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Play
{
    internal class PacketConfirmTransaction : Packet
    {
        public bool Accepted { get; set; }
        public byte WindowId;
        public short ActionId;
        public virtual void HandlePacket(IHandler handler)
        {
            handler.HandlePacketConfirmTransaction(this);
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            WindowId = buff.ReadByte();
            ActionId = buff.ReadShort();
            Accepted = buff.ReadBool();
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            buff.WriteByte(WindowId);
            buff.WriteShort(ActionId);
            buff.WriteBool(Accepted);
        }
    }
}