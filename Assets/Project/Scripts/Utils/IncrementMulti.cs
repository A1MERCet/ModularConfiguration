using System;

namespace Project
{
    [Serializable]
    public class IncrementMulti : Increment
    {
        public override float Count()
        {
            cache = defaultValue;
            foreach (float p in _increments.Values)
                cache *= p;
            return cache;
        }
    }
}