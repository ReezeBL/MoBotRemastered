using System.Collections.Generic;

namespace MoBot.Helpers
{
    internal class LimitedDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public int MaxCapacity { get; set; } = 1024;

        private readonly Queue<TKey> orderedKeys = new Queue<TKey>();

        public new void Add(TKey key, TValue value)
        {
            orderedKeys.Enqueue(key);
            if (MaxCapacity != 0 && Count >= MaxCapacity)
            {
                Remove(orderedKeys.Dequeue());
            }
            base.Add(key, value);
        }
    }
}