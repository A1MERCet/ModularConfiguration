using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [Serializable]
    public class IncrementVector3
    {
        
        [SerializeField] private SerializableDic<string,Vector3> _increments = new();
        public Vector3 defaultValue = new();
        public SerializableDic<string,Vector3> Get()            {return _increments;}
        public Vector3 Get(string v)                            {return _increments[v];}
        public IncrementVector3 Add(string v , ref Vector3 p)   {_increments[v]=p; Count();return this; }
        public IncrementVector3 Add(string v)                   {_increments[v]=Vector3.zero; Count();return this; }
        public Vector3 Remove(string v)                         { Vector3 p = _increments[v];_increments.Remove(v);Count();return p; }

        private Vector3 cache = new();
        public Vector3 Cache => cache;

        public Vector3 Count()
        {
            cache.Set(defaultValue.x,defaultValue.y,defaultValue.z);
            foreach (Vector3 p in _increments.Values)
                 cache += p;
            return cache;
        }

        public Vector3 this[string id]
        {
            get => _increments[id];
            set => _increments[id] = value;
        }

    }
}