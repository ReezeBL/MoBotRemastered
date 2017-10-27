using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using fNbt;
using MoBot.Core.GameData.Entities;
using MoBot.Core.GameData.World;
using MoBot.Core.Pathfinder;

namespace MoBot.Core
{
    public class GameController
    {
        public GameWorld World { get; } = new GameWorld();
        public Player Player { get; private set; }
        public IList<Entity> LivingEntities { get; } = new BindingList<Entity>();


        private readonly ConcurrentDictionary<int, Entity> entities = new ConcurrentDictionary<int, Entity>();

        public Entity GetEntity<T>() where T : Entity
        {
            return entities.Values.OfType<T>().FirstOrDefault();
        }

        public Entity GetEntity(int id)
        {
            entities.TryGetValue(id, out var res);
            return res;
        }

        public IEnumerable<T> GetEntities<T>()
        {
            return entities.Values.OfType<T>();
        }

        public void CreateUser(int uid, string name = "")
        {
            Player = CreatePlayer(uid, name);
        }

        public Player CreatePlayer(int uid, string name)
        {
            var player = new Player(uid, name);
            LivingEntities.Add(player);
            if (entities.TryAdd(uid, player)) return player;
            Console.WriteLine($"Cannot add Entity {uid} to collection!");
            return null;
        }

        public Mob CreateMob(int entityId, byte type = 0)
        {
            var entity = new Mob(entityId) { Type = type };
            LivingEntities.Add(entity);
            if (entities.TryAdd(entityId, entity)) return entity;
            Console.WriteLine($"Cannot add Entity {entityId} to collection!");
            return null;
        }

        public LivingEntity CreateLivingEntity(int entityId, byte type)
        {
            var entity = new LivingEntity(entityId);
            if (entities.TryAdd(entityId, entity)) return entity;
            Console.WriteLine($"Cannot add Entity {entityId} to collection!");
            return null;
        }

        public Entity CreateEntity(int entityId, byte type)
        {
            var entity = new Entity(entityId);
            if (entities.TryAdd(entityId, entity)) return entity;
            Console.WriteLine($"Cannot add Entity {entityId} to collection!");
            return null;
        }

        public void SetTileEntity(Location location, NbtCompound root)
        {
            //TODO: Support tile entities system!
        }
    }
}