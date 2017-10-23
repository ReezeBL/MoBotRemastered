using System;
using MoBot.Core.GameData;
using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Play
{
    internal class PacketWindowItems : Packet
    {
        public byte WindowId;
        public short ItemCount;
        public ItemStack[] ItemsStack;

        public override void ReadPacketData(StreamWrapper buff)
        {
            WindowId = buff.ReadByte();
            ItemCount = buff.ReadShort();
            ItemsStack = new ItemStack[ItemCount];
            for(var i = 0; i < ItemCount; i++)
            {
                ItemsStack[i] = ReadItem(buff);
            }
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            throw new NotImplementedException();
        }

        public override void HandlePacket(IHandler handler)
        {
            handler.HandlePacketWindowItems(this);
        }
    }
}
