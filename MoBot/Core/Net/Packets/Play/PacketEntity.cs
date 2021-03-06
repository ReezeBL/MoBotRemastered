﻿using System;
using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Play
{
    internal class PacketEntity : Packet
    {
        public int EntityId;
        public double X, Y, Z;

        public virtual void HandlePacket(IHandler handler)
        {
            handler.HandlePacketEntity(this);
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            EntityId = buff.ReadInt();
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            throw new NotImplementedException();
        }

        public class PacketEntityMove : PacketEntity
        {
            public override void ReadPacketData(StreamWrapper buff)
            {
                base.ReadPacketData(buff);
                X = (sbyte)buff.ReadByte() / 32.0;
                Y = (sbyte)buff.ReadByte() / 32.0;
                Z = (sbyte)buff.ReadByte() / 32.0;
            }
        }
    }
}
