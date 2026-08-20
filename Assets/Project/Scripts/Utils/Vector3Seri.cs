using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
[JsonObject(MemberSerialization.OptIn)]
public struct Vector3Seri
{
    [JsonProperty] public float x;
    [JsonProperty] public float y;
    [JsonProperty] public float z;
    
    public Vector3 ToVector3() => new Vector3(x, y, z);
    public void FromVector3(Vector3 v) { x = v.x; y = v.y; z = v.z; }
    public override string ToString() => $"({x}, {y}, {z})";
}