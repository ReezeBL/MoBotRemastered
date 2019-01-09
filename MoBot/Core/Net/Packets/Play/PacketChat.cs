using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Play
{
    internal class PacketChat : Packet
    {
        public string Message;
        public virtual void HandlePacket(IHandler handler)
        {
            handler.HandlePacketChat(this);
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            Message = buff.ReadString();
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            buff.WriteString(Message);
        }
    }
}
