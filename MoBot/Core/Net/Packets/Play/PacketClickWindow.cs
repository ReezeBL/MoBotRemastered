using System;
using MoBot.Core.GameData;
using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Play
{
    internal class PacketClickWindow : Packet
    {
        public byte WindowId;
        public short Slot;
        public byte Button;
        public short ActionNumber;
        public byte Mode;
        public ItemStack ItemStack;
        public virtual void HandlePacket(IHandler handler)
        {
            throw new NotImplementedException();
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            throw new NotImplementedException();
        }

        public override void WritePacketData(StreamWrapper buff)
        {           
            buff.WriteByte(WindowId);
            buff.WriteShort(Slot);
            buff.WriteByte(Button);
            buff.WriteShort(ActionNumber);
            buff.WriteByte(Mode);
            WriteItem(buff, ItemStack);
        }
    }
}
