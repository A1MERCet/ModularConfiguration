using System;
using UnityEngine;

namespace Project
{
    [Serializable]
    public class Increment
    {
        [SerializeField] protected SerializableDic<string,float> _increments = new();
        public float defaultValue;
        public SerializableDic<string,float> Get()            {return _increments;}
        public float Get(string v)                            {return _increments[v];}
        public Increment Add(string v , float p)              {_increments[v]=p; Count();return this; }
        public Increment Add(string v)                        {_increments[v]=0F; Count();return this; }
        public float Remove(string v)                         { float p = _increments[v];_increments.Remove(v);Count();return p; }

        protected float cache;
        public float Cache => cache;

        public virtual float Count()
        {
            cache = defaultValue;
            foreach (float p in _increments.Values)
                 cache += p;
            return cache;
        }
        public float this[string id]
        {
            get => _increments[id];
            set => _increments[id] = value;
        }

    }
}