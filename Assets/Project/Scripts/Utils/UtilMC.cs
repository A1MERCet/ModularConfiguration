using UnityEngine;

public class UtilMC
{
    public static Vector3 Location2Vector(Vector3 v) => new Vector3(v.z, v.y, v.x) * 0.01F;
    public static Vector3 Vector2Location(Vector3 v) => new Vector3(v.z, v.y, v.x) * 100F;
    
    public static Vector3 Rotation2Vector(Vector3 v) => new Vector3(v.x, v.y, v.z);
    public static Vector3 Vector2Rotation(Vector3 v) => new Vector3(v.x, v.y, v.z);
}