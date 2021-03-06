﻿using System;
using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Play
{
    internal class PacketOpenWindow : Packet
    {
        public int WorldId;
        public int WindowId;
        public string WindowName;
        public int SlotNumber;
        public bool HasCustomInventory;
        public int EntityId;

        public virtual void HandlePacket(IHandler handler)
        {
            handler.HandlePacketOpenWindow(this);
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            WindowId = buff.ReadByte();
            WorldId = buff.ReadByte();
            WindowName = buff.ReadString();
            SlotNumber = buff.ReadByte();
            HasCustomInventory = buff.ReadBool();

            if (WindowId == 11)
                EntityId = buff.ReadInt();
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            throw new NotImplementedException();
        }
    }
}
