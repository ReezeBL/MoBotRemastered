using System;
using System.Collections.Generic;
using System.Net;
using MoBot.Core.Net.Packets;
using MoBot.Core.Net.Packets.Handshake;
using MoBot.Core.Net.Packets.Play;
using MoBot.Core.Plugins;
using MoBot.Helpers;
using NLog;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace MoBot.Core.Net.Handlers
{
    internal class ClientHandler : IHandler
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<string , ICustomHandler> customPayloadHandlers = new Dictionary<string, ICustomHandler>();
        private readonly MoBase baseInstance;

        public ClientHandler(MoBase baseInstance)
        {
            this.baseInstance = baseInstance;
        }

        public void RegisterCustomHandler(string channel, ICustomHandler customHandler)
        {
            customPayloadHandlers.Add(channel, customHandler);
        }

        public void HandlePacketBlockChange(PacketBlockChange packetBlockChange)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketChat(PacketChat packetChat)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketChunkData(PacketChunkData packetChunkData)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketConfirmTransaction(PacketConfirmTransaction packetConfirmTransaction)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketCustomPayload(PacketCustomPayload packetCustomPayload)
        {
            if(customPayloadHandlers.TryGetValue(packetCustomPayload.Channel, out ICustomHandler handler))
                handler.ProcessPacketData(packetCustomPayload.Payload);
        }

        public void HandlePacketDestroyEntities(PacketDestroyEntities packetDestroyEntities)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketDisconnect(PacketDisconnect packetDisconnect)
        {
            baseInstance.OnNotify($"Disconnected from server: {packetDisconnect.Reason}");
            baseInstance.Disconnect();
        }

        public void HandlePacketEncriptionRequest(PacketEncriptionRequest packetEncriptionRequest)
        {
            var kp = PublicKeyFactory.CreateKey(packetEncriptionRequest.Key);
            var key = (RsaKeyParameters)kp;
            var cipher = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            cipher.Init(true, key);
            var keygen = new CipherKeyGenerator();
            keygen.Init(new KeyGenerationParameters(new SecureRandom(), 128));
            var secretKey = keygen.GenerateKey();
            var cryptedKey = cipher.DoFinal(secretKey);
            var cryptedToken = cipher.DoFinal(packetEncriptionRequest.Token);

            try
            {
                var serverId = HashHelper.GetServerIdHash(packetEncriptionRequest.ServerId, secretKey,
                    packetEncriptionRequest.Key);

                if (!GlobalModules.AuthHandler.HandleAuth(baseInstance.UserSettings.Username,
                    baseInstance.UserSettings.UserToken, serverId))
                {
                    Logger.Error($"Auth failed!\nUsername: {baseInstance.UserSettings.Username}\nToken: {baseInstance.UserSettings.UserToken}");
                }
                //var responseString = HttpHelper.PostUrl(baseInstance.UserSettings.Username,
                //    baseInstance.UserSettings.UserToken, serverId);

                //if (responseString != "OK")
                //    Logger.Error("Auth failed!\nAuth username: {1}\nAuth ID:{2}\nAuth response : {0}", responseString,
                //        baseInstance.UserSettings.Username, baseInstance.UserSettings.UserToken);
            }
            catch (WebException)
            {
                Logger.Error("Unable to connect to login server!");
            }


            baseInstance.NetworkManager.AddToSendingQueue(new PacketEncriptionResponse
            {
                SharedSecret = cryptedKey,
                SharedSecretLength = cryptedKey.Length,
                Token = cryptedToken,
                TokenLength = cryptedToken.Length
            }, true);

            baseInstance.NetworkManager.Channel.EncriptChannel(secretKey);
        }

        public void HandlePacketEntity(PacketEntity packetEntity)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketEntityStatus(PacketEntityStatus packetEntityStatus)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketEntityTeleport(PacketEntityTeleport packetEntityTeleport)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketHeldItemChange(PacketHeldItemChange packetHeldItemChange)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketJoinGame(PacketJoinGame packetJoinGame)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketKeepAlive(PacketKeepAlive packetKeepAlive)
        {
            baseInstance.NetworkManager.AddToSendingQueue(packetKeepAlive);
        }

        public void HandlePacketLoginSucess(PacketLoginSuccess packetLoginSuccess)
        {
            baseInstance.NetworkManager.Channel.ChangeState(Channel.State.Play);
        }

        public void HandlePacketMapChunk(PacketMapChunk packetMapChunk)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketMultiBlockChange(PacketMultiBlockChange packetMultiBlockChange)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketPlayerAbliities(PacketPlayerAbilities packetPlayerAbilities)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketPlayerPosLook(PacketPlayerPosLook packetPlayerPosLook)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketSetSlot(PacketSetSlot packetSetSlot)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketSpawnMoob(PacketSpawnMob packetSpawnMob)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketSpawnObject(PacketSpawnObject packetSpawnObject)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketSpawnPlayer(PacketSpawnPlayer packetSpawnPlayer)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketUpdateHealth(PacketUpdateHealth packetUpdateHelath)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketWindowItems(PacketWindowItems packetWindowItems)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketRespawn(PacketRespawn packetRespawn)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketOpenWindow(PacketOpenWindow packetOpenWindow)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketCloseWindow(PacketCloseWindow packetCloseWindow)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketUpdateTileEntity(PacketUpdateTileEntity packetUpdateTileEntity)
        {
            throw new NotImplementedException();
        }
    }
}
