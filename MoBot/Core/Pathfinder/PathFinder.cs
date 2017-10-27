using System;
using System.Collections.Generic;
using MoBot.Core.Game.AI.Pathfinding;
using MoBot.Core.GameData;
using NLog;
using Priority_Queue;

namespace MoBot.Core.Pathfinder
{
    internal static class PathFinder
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Dictionary<int, Location> PointSet = new Dictionary<int, Location>();
        private static readonly Dictionary<int, float> WeightSet = new Dictionary<int, float>();

        private static Location CreatePoint(int x, int y, int z)
        {
            var hash = Location.CalcHash(x, y, z);
            if (PointSet.TryGetValue(hash, out Location result))
                return result;
            result = new Location(x, y, z);
            return PointSet[hash] = result;
        }

        public static Path Shovel(Location start, Location end, bool includeLast = true, bool digBlocs = true, float maxDistance = 64f)
        {
            try
            {
                var nodes = new FastPriorityQueue<Location>((int)maxDistance * (int)maxDistance * 255);
                var cost = new Dictionary<Location, float>();
                var succeed = false;

                nodes.Enqueue(start, 0);
                start.Prev = null;
                cost.Add(start, 0f);

                var endCost = GetBlockWeight(end.X, end.Y, end.Z) + GetBlockWeight(end.X, end.Y + 1, end.Z);
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
                    foreach (var node in AdvancedNeighbours(current))
                    {
                        var next = node.Item1;
                        if (next.Equals(end) && !includeLast)
                        {
                            end = current;
                            succeed = true;
                            break;
                        }

                        if(node.Item2 < 0) continue;
                        if(node.Item1.DistanceTo(start) > maxDistance) continue;
                        if(!digBlocs && node.Item2 > 0) continue;

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
                    if(succeed)
                        break;
                }
                if (!succeed)
                {
                    PointSet.Clear();
                    WeightSet.Clear();
                    GC.Collect();
                    return null;
                }

                var pathfrom = new List<Location>();

                while (end.Prev != null)
                {
                    pathfrom.Add(end);
                    end = end.Prev;
                }

                PointSet.Clear();
                WeightSet.Clear();
                GC.Collect();

                pathfrom.Reverse();
                return new Path(pathfrom);
            }
            catch (Exception e)
            {
                Log.Error($"Cant create path! Error : {e}");
                PointSet.Clear();
                return null;
            }
        }

        public static IEnumerable<Tuple<Location, float>> AdvancedNeighbours(Location root)
        {

            yield return Tuple.Create(CreatePoint(root.X + 1, root.Y, root.Z),
                GetBlockWeight(root.X + 1, root.Y, root.Z) + GetBlockWeight(root.X + 1, root.Y + 1, root.Z));
            yield return Tuple.Create(CreatePoint(root.X - 1, root.Y, root.Z),
                GetBlockWeight(root.X - 1, root.Y, root.Z) + GetBlockWeight(root.X - 1, root.Y + 1, root.Z));
            yield return Tuple.Create(CreatePoint(root.X, root.Y, root.Z + 1),
                GetBlockWeight(root.X, root.Y, root.Z + 1) + GetBlockWeight(root.X, root.Y + 1, root.Z + 1));
            yield return Tuple.Create(CreatePoint(root.X, root.Y, root.Z - 1),
                GetBlockWeight(root.X, root.Y, root.Z - 1) + GetBlockWeight(root.X, root.Y + 1, root.Z - 1));
            yield return Tuple.Create(CreatePoint(root.X, root.Y + 1, root.Z), GetBlockWeight(root.X, root.Y + 2, root.Z));

            if (root.Y > 0)
            {
                yield return Tuple.Create(CreatePoint(root.X, root.Y - 1, root.Z), GetBlockWeight(root.X, root.Y - 1, root.Z));
            }
        }

        private static float GetBlockWeight(int x, int y, int z)
        {
            var hash = Location.CalcHash(x, y, z);
            if (WeightSet.TryGetValue(hash, out float weight))
                return weight;

            Block block = null;//Block.GetBlock(GameController.World.GetBlock(x, y, z));
            if (block.Transparent)
                return WeightSet[hash] = 0;
            if (block.Hardness < 0)
                return WeightSet[hash] = - 1e9f;
            return WeightSet[hash] = block.Hardness * 5;
        }

        internal class Context
        {
            
        }
    }
}
