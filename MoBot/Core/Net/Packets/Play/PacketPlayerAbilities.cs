﻿using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Play
{
    internal class PacketPlayerAbilities : Packet
    {
        public byte Flags;
        public float FlyingSpeed;
        public float ViewModifier;
        public virtual void HandlePacket(IHandler handler)
        {
            handler.HandlePacketPlayerAbliities(this);
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            Flags = buff.ReadByte();
            FlyingSpeed = buff.ReadSingle();
            ViewModifier = buff.ReadSingle();
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            buff.WriteByte(Flags);
            buff.WriteSingle(FlyingSpeed);
            buff.WriteSingle(ViewModifier);
        }
    }
}
