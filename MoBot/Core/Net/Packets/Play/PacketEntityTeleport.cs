﻿using System;
using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Play
{
    internal class PacketEntityTeleport : Packet
    {
        public int EntityId;
        public double X, Y, Z;
        public virtual void HandlePacket(IHandler handler)
        {
            handler.HandlePacketEntityTeleport(this);
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            EntityId = buff.ReadInt();
            X = buff.ReadInt() / 32.0;
            Y = buff.ReadInt() / 32.0;
            Z = buff.ReadInt() / 32.0;
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            throw new NotImplementedException();
        }
    }
}
