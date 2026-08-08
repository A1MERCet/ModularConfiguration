using System;
using UnityEngine;

namespace Project
{
    [Serializable]
    public class IncrementPos
    {
        [Serializable] public class PosDictionary : UnitySerializedDictionary<string,Pos> { }

        [Serializable]
        public class Pos
        {
            [SerializeField] public Vector3 position = Vector3.zero;
            [SerializeField] public Vector3 rotation = Vector3.zero;
            public Quaternion Quaternion => Quaternion.Euler(rotation);
            public Pos SetPos(Vector3 v)
            {
                position.x = v.x;
                position.y = v.y;
                position.z = v.z;
                return this;
            }
            public Pos SetRot(Vector3 v)
            {
                rotation.x = v.x;
                rotation.y = v.y;
                rotation.z = v.z;
                return this;
            }
            public Pos SetPos(float x, float y, float z)
            {
                position.x = x;
                position.y = y;
                position.z = z;
                return this;
            }
            public Pos SetRot(float x, float y, float z)
            {
                rotation.x = x;
                rotation.y = y;
                rotation.z = z;
                return this;
            }

            public static Pos operator+(Pos a, Pos b)
            {
                a.position += b.position;
                a.rotation += b.rotation;
                return a;
            }
            public static Pos operator-(Pos a, Pos b)
            {
                a.position -= b.position;
                a.rotation -= b.rotation;
                return a;
            }
            public static Pos operator*(Pos a, float v)
            {
                a.position *= v;
                a.rotation *= v;
                return a;
            }
            public static Pos operator/(Pos a, float v)
            {
                a.position /= v;
                a.rotation /= v;
                return a;
            }

            public override string ToString()
            {
                return $"Position:{position}Rotation:{rotation}";
            }
        }
        
        [SerializeField] private PosDictionary _increments = new();
        [SerializeField] public Pos defaultValue = new();
        public PosDictionary Get()                          {return _increments;}
        public Pos Get(string v)                            {return _increments[v];}
        public IncrementPos Add(string v , ref Pos p)       {_increments[v]=p; Count();return this; }
        public IncrementPos Add(string v)                   {_increments[v]=new Pos(); Count();return this; }
        public Pos Remove(string v)                         { Pos p = _increments[v];_increments.Remove(v);Count();return p; }

        private Pos cache = new();
        public Pos Cache => cache;

        public Pos Count()
        {
            cache.SetPos(defaultValue.position).SetRot(defaultValue.rotation);
            foreach (Pos p in _increments.Values)
                 cache += p;
            return cache;
        }

        public Pos this[string id]
        {
            get => _increments[id];
            set => _increments[id] = value;
        }

    }
}