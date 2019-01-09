using MoBot.Core.Net.Handlers;

namespace MoBot.Core.Net.Packets.Handshake
{
    internal class PacketEncriptionRequest : Packet
    {
        public byte[] Key;
        public string ServerId;
        public byte[] Token { get; set; }
        public virtual void HandlePacket(IHandler handler)
        {
            handler.HandlePacketEncriptionRequest(this);
        }

        public override void ReadPacketData(StreamWrapper buff)
        {
            ServerId = buff.ReadString();
            var length = buff.ReadShort();
            Key = buff.ReadBytes(length);
            length = buff.ReadShort();
            Token = buff.ReadBytes(length);
        }

        public override void WritePacketData(StreamWrapper buff)
        {
            
        }

        public override bool ProceedNow()
        {
            return true;
        }
    }
}