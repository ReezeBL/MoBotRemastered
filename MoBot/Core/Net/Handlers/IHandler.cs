﻿using MoBot.Core.Net.Packets;
using MoBot.Core.Net.Packets.Handshake;
using MoBot.Core.Net.Packets.Play;

namespace MoBot.Core.Net.Handlers
{
    internal interface IHandler
    {
        void RegisterCustomHandler(string channel, ICustomHandler customHandler);

        void HandlePacketBlockChange(PacketBlockChange packetBlockChange);
        void HandlePacketChat(PacketChat packetChat);
        void HandlePacketChunkData(PacketChunkData packetChunkData);
        void HandlePacketConfirmTransaction(PacketConfirmTransaction packetConfirmTransaction);
        void HandlePacketCustomPayload(PacketCustomPayload packetCustomPayload);
        void HandlePacketDestroyEntities(PacketDestroyEntities packetDestroyEntities);
        void HandlePacketDisconnect(PacketDisconnect packetDisconnect);
        void HandlePacketEncriptionRequest(PacketEncriptionRequest packetEncriptionRequest);
        void HandlePacketEntity(PacketEntity packetEntity);
        void HandlePacketEntityStatus(PacketEntityStatus packetEntityStatus);
        void HandlePacketEntityTeleport(PacketEntityTeleport packetEntityTeleport);
        void HandlePacketHeldItemChange(PacketHeldItemChange packetHeldItemChange);
        void HandlePacketJoinGame(PacketJoinGame packetJoinGame);
        void HandlePacketKeepAlive(PacketKeepAlive packetKeepAlive);
        void HandlePacketLoginSucess(PacketLoginSuccess packetLoginSuccess);
        void HandlePacketMapChunk(PacketMapChunk packetMapChunk);
        void HandlePacketMultiBlockChange(PacketMultiBlockChange packetMultiBlockChange);
        void HandlePacketPlayerAbliities(PacketPlayerAbilities packetPlayerAbilities);
        void HandlePacketPlayerPosLook(PacketPlayerPosLook packetPlayerPosLook);
        void HandlePacketSetSlot(PacketSetSlot packetSetSlot);
        void HandlePacketSpawnMoob(PacketSpawnMob packetSpawnMob);
        void HandlePacketSpawnObject(PacketSpawnObject packetSpawnObject);
        void HandlePacketSpawnPlayer(PacketSpawnPlayer packetSpawnPlayer);
        void HandlePacketUpdateHealth(PacketUpdateHealth packetUpdateHelath);
        void HandlePacketWindowItems(PacketWindowItems packetWindowItems);
        void HandlePacketRespawn(PacketRespawn packetRespawn);
        void HandlePacketOpenWindow(PacketOpenWindow packetOpenWindow);
        void HandlePacketCloseWindow(PacketCloseWindow packetCloseWindow);
        void HandlePacketUpdateTileEntity(PacketUpdateTileEntity packetUpdateTileEntity);
    }
}