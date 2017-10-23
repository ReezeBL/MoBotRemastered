using System;
using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Play
{
    internal class PacketUpdateHealth : Packet
    {
        public float Health;
        public short Food;
        public float Saturation;
        public override void HandlePacket(IHandler handler)
        {
            handler.HandlePacketUpdateHealth(this);
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            Health = buff.ReadSingle();
            Food = buff.ReadShort();
            Saturation = buff.ReadSingle();
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            throw new NotImplementedException();
        }
    }
}
