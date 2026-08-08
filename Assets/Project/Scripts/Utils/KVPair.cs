using System;
using UnityEngine;

[Serializable]
public struct KVPair<K,V>
{
    
    [SerializeField] private K key;
    [SerializeField] private V value;

    public KVPair( K key,V value)
    {
        this.key = key;
        this.value = value;
    }

    public K Key()   => key;
    public V Value() => value;
    public KVPair<K,V> SetKey(K k)   { key = k;return this; }
    public KVPair<K,V> SetValue(V v) { value = v; return this; }
    
}