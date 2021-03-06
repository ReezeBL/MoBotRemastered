﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using MoBot.Core.GameData.Entities;
using MoBot.Core.Net.Packets;
using MoBot.Core.Net.Packets.Handshake;
using MoBot.Core.Net.Packets.Play;
using MoBot.Core.Pathfinder;
using MoBot.Core.Plugins;
using MoBot.Helpers;
using Newtonsoft.Json.Linq;
using NLog;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace MoBot.Core.Net.Handlers
{
    public class ClientHandler
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<string, ICustomHandler> customPayloadHandlers =
            new Dictionary<string, ICustomHandler>();
        
        private readonly Dictionary<Type, Action<object>> callbacks = new Dictionary<Type, Action<object>>();
        
        /* private readonly MoBase baseInstance; */

        internal ClientHandler()
        {
            
        }

        public void RegisterCustomHandler(string channel, ICustomHandler customHandler)
        {
            customPayloadHandlers.Add(channel, customHandler);
        }

        public void RegisterPacketCallback<T>(Action<T> handler) where T : Packet
        {
            var packetType = typeof(T);
            callbacks.TryGetValue(packetType, out var callback);
            callback += o => handler((T)o);
            callbacks[packetType] = callback;
        }

        internal void ProcessPacket(Packet packet)
        {
            var packetType = packet.GetType();
            if(callbacks.TryGetValue(packetType, out var callback))
                callback?.Invoke(packet);
        }

        /*public void HandlePacketBlockChange(PacketBlockChange packetBlockChange)
        {
            baseInstance.IngameData.World.SetBlock(packetBlockChange.X, packetBlockChange.Y, packetBlockChange.Z,
                packetBlockChange.BlockId);
            baseInstance.IngameData.World.Invalidate();
        }

        public void HandlePacketChat(PacketChat packetChat)
        {
            dynamic response = JObject.Parse(packetChat.Message);
            var stringBuilder = new StringBuilder();
            if (response.extra != null)
            {
                
                foreach (var obj in (JArray)response.extra)
                {
                    if (obj is JValue)
                    {
                        stringBuilder.Append(obj);
                    }
                    else if (obj != null)
                    {
                        stringBuilder.Append($"{obj.Value<string>("text")}");
                    }
                }
            }
            else
            {
                stringBuilder.Append(packetChat.Message);
            }
            baseInstance.OnNotify(stringBuilder.ToString());
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
            foreach (var entityId in packetDestroyEntities.IdList)
            {
                baseInstance.IngameData.RemoveEntity(entityId);
            }
        }

        public void HandlePacketDisconnect(PacketDisconnect packetDisconnect)
        {
            baseInstance.OnNotify($"Disconnected from server: {packetDisconnect.Reason}");
            baseInstance.Disconnect(packetDisconnect.Reason);
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
            var entity = baseInstance.IngameData.GetEntity(packetEntity.EntityId) as LivingEntity;
            entity?.Move(packetEntity.X, packetEntity.Y, packetEntity.Z);
        }

        public void HandlePacketEntityStatus(PacketEntityStatus packetEntityStatus)
        {
            throw new NotImplementedException();
        }

        public void HandlePacketEntityTeleport(PacketEntityTeleport packetEntityTeleport)
        {
            var entity = baseInstance.IngameData.GetEntity(packetEntityTeleport.EntityId) as LivingEntity;
            entity?.SetPosition(packetEntityTeleport.X, packetEntityTeleport.Y, packetEntityTeleport.Z);
        }

        public void HandlePacketHeldItemChange(PacketHeldItemChange packetHeldItemChange)
        {
            baseInstance.IngameData.Player.HeldItemBar = packetHeldItemChange.Slot;
        }

        public void HandlePacketJoinGame(PacketJoinGame packetJoinGame)
        {
            baseInstance.IngameData.CreateUser(packetJoinGame.EntityId, baseInstance.UserSettings.Username);
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
            var mas = new byte[packetMapChunk.DataLength - 2];
            Array.Copy(packetMapChunk.ChunkData, 2, mas, 0, packetMapChunk.DataLength - 2);
            var dced = Decompressor.Decompress(mas);

            for (var i = 0; i < packetMapChunk.ChunkNumber; i++)
            {
                dced = packetMapChunk.Chunks[i].GetData(dced);
                baseInstance.IngameData.World.AddChunk(packetMapChunk.Chunks[i]);
            }
        }

        public void HandlePacketMultiBlockChange(PacketMultiBlockChange packetMultiBlockChange)
        {
            var chunkX = packetMultiBlockChange.ChunkXPosiiton * 16;
            var chunkZ = packetMultiBlockChange.ChunkZPosition * 16;

            if (packetMultiBlockChange.Metadata == null) return;

            var buff = new StreamWrapper(packetMultiBlockChange.Metadata);
            for (var i = 0; i < packetMultiBlockChange.Size; i++)
            {
                var short1 = buff.ReadShort();
                var short2 = buff.ReadShort();
                var id = short2 >> 4 & 4095;
                var x = short1 >> 12 & 15;
                var z = short1 >> 8 & 15;
                var y = short1 & 255;
                baseInstance.IngameData.World.SetBlock(chunkX + x, y, chunkZ + z, id);
            }
        }

        public void HandlePacketPlayerAbliities(PacketPlayerAbilities packetPlayerAbilities)
        {
            //TODO: WTF???
        }

        public void HandlePacketPlayerPosLook(PacketPlayerPosLook packetPlayerPosLook)
        {
            baseInstance.IngameData.Player.SetPosition(packetPlayerPosLook.X, packetPlayerPosLook.Y -= 1.62,
                packetPlayerPosLook.Z);
            baseInstance.IngameData.Player.Yaw = packetPlayerPosLook.Yaw;
            baseInstance.IngameData.Player.Pitch = packetPlayerPosLook.Pitch;
            baseInstance.IngameData.Player.OnGround = packetPlayerPosLook.OnGround;

            //GameController.AiHandler.Mover.Stop();
            baseInstance.NetworkManager.AddToSendingQueue(packetPlayerPosLook);
        }

        public void HandlePacketSetSlot(PacketSetSlot packetSetSlot)
        {
            try
            {
                baseInstance.IngameData.Player.GetContainer(packetSetSlot.WindowId)[packetSetSlot.Slot] =
                    packetSetSlot.ItemStack;
            }
            catch (Exception e)
            {
                Logger
                    .Warn($"Failed to set slot {packetSetSlot.Slot} in window {packetSetSlot.WindowId}, exception {e}");
            }
        }

        public void HandlePacketSpawnMoob(PacketSpawnMob packetSpawnMob)
        {
            var mob = baseInstance.IngameData.CreateMob(packetSpawnMob.EntityId, packetSpawnMob.Type);
            mob?.SetPosition(packetSpawnMob.X, packetSpawnMob.Y, packetSpawnMob.Z);
        }

        public void HandlePacketSpawnObject(PacketSpawnObject packetSpawnObject)
        {
            var entity = baseInstance.IngameData.CreateEntity(packetSpawnObject.EntityId, packetSpawnObject.Type);
            entity?.SetPosition(packetSpawnObject.X, packetSpawnObject.Y, packetSpawnObject.Z);
        }

        public void HandlePacketSpawnPlayer(PacketSpawnPlayer packetSpawnPlayer)
        {
            var player = baseInstance.IngameData.CreatePlayer(packetSpawnPlayer.EntityId, packetSpawnPlayer.Name);
            player?.SetPosition(packetSpawnPlayer.X, packetSpawnPlayer.Y, packetSpawnPlayer.Z);
        }

        public void HandlePacketUpdateHealth(PacketUpdateHealth packetUpdateHelath)
        {
            baseInstance.IngameData.Player.Health = packetUpdateHelath.Health;
            baseInstance.IngameData.Player.Food = packetUpdateHelath.Food;
            baseInstance.IngameData.Player.Saturation = packetUpdateHelath.Saturation;
        }

        public void HandlePacketWindowItems(PacketWindowItems packetWindowItems)
        {
            var container = baseInstance.IngameData.Player.GetContainer(packetWindowItems.WindowId) ??
                            baseInstance.IngameData.Player.CreateContainer(packetWindowItems.WindowId,
                                packetWindowItems.ItemCount - 36);
            for (var i = 0; i < packetWindowItems.ItemCount; i++)
                container[i] = packetWindowItems.ItemsStack[i];
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
            baseInstance.IngameData.SetTileEntity(
                new Location(packetUpdateTileEntity.X, packetUpdateTileEntity.Y, packetUpdateTileEntity.Z),
                packetUpdateTileEntity.Root);
        }*/
    }
}