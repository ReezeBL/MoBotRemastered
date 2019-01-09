using System;
using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets
{
    internal class PacketDisconnect : Packet
    {
        public string Reason;
        public virtual void HandlePacket(IHandler handler)
        {
            handler.HandlePacketDisconnect(this);
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            Reason = buff.ReadString();
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            throw new NotImplementedException();
        }

        public override bool ProceedNow()
        {
            return true;
        }
    }
}
