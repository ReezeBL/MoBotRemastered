using System;
using System.Collections.Generic;
using MoBot.Core.Game.AI.Pathfinding;
using MoBot.Core.GameData;
using MoBot.Core.GameData.World;
using MoBot.Helpers;
using NLog;
using Priority_Queue;

namespace MoBot.Core.Pathfinder
{
    internal static class PathFinder
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public static PathfinderContext Context { get; set; }

        public static Path Shovel(Location start, Location end, bool includeLast = true, bool digBlocs = true, float maxDistance = 64f, PathfinderContext context = null)
        {
            if (context == null)
                context = Context;
            try
            {
                var nodes = new FastPriorityQueue<Location>((int) maxDistance * (int) maxDistance * 255);
                var cost = new Dictionary<Location, float>();
                var succeed = false;

                nodes.Enqueue(start, 0);
                start.Prev = null;
                cost.Add(start, 0f);

                var endCost = context.GetBlockWeight(end.X, end.Y, end.Z) +
                              context.GetBlockWeight(end.X, end.Y + 1, end.Z);
                if (endCost < 0)
                    return null;

                while (nodes.Count > 0)
                {
                    var current = nodes.Dequeue();
                    if (current.Equals(end))
                    {
                        end = current;
                        succeed = true;
                        break;
                    }
                    foreach (var node in AdvancedNeighbours(current, context))
                    {
                        var next = node.Item1;
                        if (next.Equals(end) && !includeLast)
                        {
                            end = current;
                            succeed = true;
                            break;
                        }

                        if (node.Item2 < 0) continue;
                        if (node.Item1.DistanceTo(start) > maxDistance) continue;
                        if (!digBlocs && node.Item2 > 0) continue;

                        var nodeCost = cost[current] + 1f + node.Item2;
                        if (!cost.TryGetValue(next, out float nextCost))
                        {
                            cost.Add(next, nodeCost);
                            nodes.Enqueue(next, nodeCost + next.DistanceTo(end));
                            next.Prev = current;
                        }
                        else if (nextCost > nodeCost)
                        {
                            cost[next] = nodeCost;
                            nodes.UpdatePriority(next, nodeCost + next.DistanceTo(end));
                            next.Prev = current;
                        }
                    }
                    if (succeed)
                        break;
                }
                if (!succeed)
                    return null;

                var pathfrom = new List<Location>();

                while (end.Prev != null)
                {
                    pathfrom.Add(end);
                    end = end.Prev;
                }

                pathfrom.Reverse();
                return new Path(pathfrom);
            }
            catch (Exception e)
            {
                Log.Error($"Cant create path! Error : {e}");
                return null;
            }
            finally
            {
                context.ResetBuffers();
                GC.Collect();
            }
        }

        private static IEnumerable<Tuple<Location, float>> AdvancedNeighbours(Location root, PathfinderContext context)
        {

            yield return Tuple.Create(context.CreatePoint(root.X + 1, root.Y, root.Z),
                context.GetBlockWeight(root.X + 1, root.Y, root.Z) + context.GetBlockWeight(root.X + 1, root.Y + 1, root.Z));
            yield return Tuple.Create(context.CreatePoint(root.X - 1, root.Y, root.Z),
                context.GetBlockWeight(root.X - 1, root.Y, root.Z) + context.GetBlockWeight(root.X - 1, root.Y + 1, root.Z));
            yield return Tuple.Create(context.CreatePoint(root.X, root.Y, root.Z + 1),
                context.GetBlockWeight(root.X, root.Y, root.Z + 1) + context.GetBlockWeight(root.X, root.Y + 1, root.Z + 1));
            yield return Tuple.Create(context.CreatePoint(root.X, root.Y, root.Z - 1),
                context.GetBlockWeight(root.X, root.Y, root.Z - 1) + context.GetBlockWeight(root.X, root.Y + 1, root.Z - 1));
            yield return Tuple.Create(context.CreatePoint(root.X, root.Y + 1, root.Z), context.GetBlockWeight(root.X, root.Y + 2, root.Z));

            if (root.Y > 0)
            {
                yield return Tuple.Create(context.CreatePoint(root.X, root.Y - 1, root.Z), context.GetBlockWeight(root.X, root.Y - 1, root.Z));
            }
        }

        public class PathfinderContext
        {
            public PathfinderContext(GameWorld world)
            {
                World = world;
            }

            private GameWorld World { get; }
            private Dictionary<int, Location> Locations { get; } = new Dictionary<int, Location>(8*64*64);
            private Dictionary<int, float> Weights { get; } = new Dictionary<int, float>(8*64*64);

            public void ResetBuffers()
            {
                Locations.Clear();
                Weights.Clear();
            }

            public Location CreatePoint(int x, int y, int z)
            {
                var hash = Location.CalcHash(x, y, z);
                if (Locations.TryGetValue(hash, out var result))
                    return result;
                return Locations[hash] = new Location(x, y, z);
            }

            public float GetBlockWeight(int x, int y, int z)
            {
                var hash = Location.CalcHash(x, y, z);
                if (Weights.TryGetValue(hash, out float weight))
                    return weight;

                Block block = Block.GetBlock(World.GetBlock(x, y, z));
                if (block.Transparent)
                    return Weights[hash] = 0;
                if (block.Hardness < 0)
                    return Weights[hash] = -1e9f;
                return Weights[hash] = block.Hardness * 5;
            }
        }
    }
}
