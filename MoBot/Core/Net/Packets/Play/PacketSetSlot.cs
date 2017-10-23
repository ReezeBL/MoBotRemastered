using System;
using MoBot.Core.GameData;
using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Play
{
    internal class PacketSetSlot : Packet
    {
        public byte WindowId;
        public short Slot;
        public ItemStack ItemStack;
        public override void HandlePacket(IHandler handler)
        {
            handler.HandlePacketSetSlot(this);
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            WindowId = buff.ReadByte();
            Slot = buff.ReadShort();
            ItemStack = ReadItem(buff);
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            throw new NotImplementedException();
        }
    }
}
