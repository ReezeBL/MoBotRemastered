namespace MoBot.Core.Net.Handlers
{
    public interface ICustomHandler
    {
        void ProcessPacketData(byte[] data);
    }
}
