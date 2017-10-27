using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using MoBot.Core.Net.Handlers;
using NLog;

namespace MoBot.Core.Net
{
    internal class NetworkManager
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        internal readonly Channel Channel;
        private readonly IHandler handler;

        private readonly ConcurrentQueue<Packet> toSendPackets = new ConcurrentQueue<Packet>();
        private readonly ConcurrentQueue<Packet> toProcessPackets = new ConcurrentQueue<Packet>();

        private readonly Thread readingThread;
        private readonly Thread writingThread;
        private readonly Thread processingThread;

        public bool IsRunning { get; private set; }

        internal NetworkManager(IHandler handler, Stream connectionStream, Channel.State state = Channel.State.Login)
        {
            this.handler = handler;
            Channel = new Channel(connectionStream, state);

            readingThread = new Thread(ReadCallback) {IsBackground = true};
            writingThread = new Thread(WriteCallback) {IsBackground = true};
            processingThread = new Thread(ProcessCallback) {IsBackground = true};
        }

        public void SetupThreads()
        {
            IsRunning = true;
            
            readingThread.Start();
            writingThread.Start();
            processingThread.Start();

        }

        public void StopThreads()
        {
            IsRunning = false;

            readingThread.Join(1000);
            writingThread.Join(1000);
            processingThread.Join(1000);
        }

        public void AddToSendingQueue(Packet packet, bool sendImmidiatly = false)
        {
            if(sendImmidiatly)
                Channel.SendPacket(packet);
            else
                toSendPackets.Enqueue(packet);
        }

        private void ReadCallback()
        {
            while (IsRunning)
            {
                try
                {
                    var packet = Channel.GetPacket();
                    if(packet == null)
                        continue;
                    if (packet.ProceedNow())
                        packet.HandlePacket(handler);
                    else
                        toProcessPackets.Enqueue(packet);
                }
                catch (EndOfStreamException)
                {
                    IsRunning = false;
                    Logger.Warn("Stream throtted!");
                    break;
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        private void WriteCallback()
        {
            while (IsRunning)
            {
                try
                {
                    while (!toSendPackets.IsEmpty)
                    {
                        if (toSendPackets.TryDequeue(out Packet toSendPacket))
                            Channel.SendPacket(toSendPacket);
                    }
                }
                catch (EndOfStreamException)
                {
                    //ignored
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }

                Thread.Sleep(50);
            }
        }

        private void ProcessCallback()
        {
            while (IsRunning)
            {
                try
                {
                    while (!toProcessPackets.IsEmpty)
                    {
                        if(toProcessPackets.TryDequeue(out Packet toProcessPacket))
                            toProcessPacket.HandlePacket(handler);
                    }
                }
                catch(Exception e)
                {
                    Logger.Error(e);
                }
                Thread.Sleep(50);
            }
        }
    }
}
