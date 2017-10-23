namespace MoBot.Core.GameData.Entities
{
    public class LivingEntity : Entity
    {
        private float health = 20;

        public float Yaw, Pitch;

        public LivingEntity(int entityId) : base(entityId)
        {
        }

        public float Health
        {
            get => health;
            set
            {
                health = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return $"Utyped Living Entity ({(int) X} | {(int) Y} | {(int) Z})";
        }
    }
}