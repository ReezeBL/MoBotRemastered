namespace MoBot.Core.GameData.Entities
{
    public class Mob : LivingEntity
    {
        public byte Type;

        public Mob(int id) : base(id)
        {
        }

        public override string ToString()
        {
            return $"Mob : {EntityNames[Type]} ({(int) X} | {(int) Y} | {(int) Z})";
        }
    }
}