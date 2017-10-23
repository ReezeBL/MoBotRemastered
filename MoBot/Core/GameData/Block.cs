using System.Collections.Generic;

namespace MoBot.Core.GameData
{
    public class Block
    {
        private static readonly Dictionary<int, Block> BlockRegistry = new Dictionary<int, Block>();

        public static IEnumerable<Block> Blocks => BlockRegistry.Values;

        /*public static void LoadBlocks()
        {
            try
            {
                var jsonFile = File.ReadAllText(Settings.BlocksPath);
                dynamic data = JsonConvert.DeserializeObject(jsonFile);
                foreach (var block in data)
                {
                    var gameBlock = new Block
                    {
                        Hardness = block.hardness,
                        HarvestTool = block.harvestTool,
                        Id = block.id,
                        Name = block.name,
                        Transparent = block.transparent,
                        RawName = block.rawname
                    };

                    if (block.name == "Вода")
                        Water.Add(gameBlock.Id);

                    BlockRegistry.Add(gameBlock.Id, gameBlock);
                }
            }
            catch (FileNotFoundException exception)
            {
                Program.GetLogger().Warn($"Cant find {exception.FileName} file!");   
            }
        }
*/

        public static HashSet<int> Water { get; } = new HashSet<int>();

        public static Block GetBlock(int id)
        {
            if (!BlockRegistry.TryGetValue(id, out var res))
                res = new Block { Id = id };
            return res;
        }
        
        public int Id;
        public string Name;
        public string RawName;
        public float Hardness = -1f;
        public bool Transparent;
        public string HarvestTool;

        public override string ToString()
        {
            return Id == -1 ? "" : $"{Name ?? RawName ?? ""} ({Id})";
        }
    }
}
