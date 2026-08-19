using System;
using UnityEngine;

[Serializable]
public struct Vector3Seri
{
    public float x;
    public float y;
    public float z;
    
    public Vector3 ToVector3() => new Vector3(x, y, z);
    public void FromVector3(Vector3 v) { x = v.x; y = v.y; z = v.z; }
}