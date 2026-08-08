using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UtilUnity
{
    public static bool IsInView(Camera cam, Collider collider)
    {
        if(cam == null || collider==null) return false;
        
        Vector3 center = collider.bounds.center;
        Vector3 dir    = center - cam.transform.position;

        if (Vector3.Dot(dir, cam.transform.forward) < 0) return false;

        Vector3 vp = cam.WorldToViewportPoint(center);
        if (vp.z < 0) return false;
        if (vp.x < 0 || vp.x > 1 ||
            vp.y < 0 || vp.y > 1) return false;

        Ray ray = new Ray(cam.transform.position, dir.normalized);
        // if (Physics.Raycast(ray, out RaycastHit hit))
        //     if(hit.collider != collider) return false;
        
        // Debug.DrawLine(ray.origin, ray.origin + ray.direction * hit.distance,Color.red,10F);
        
        return collider.bounds.IntersectRay(ray);
    }
    
    public static void SetLayer(GameObject obj,string layer) { SetLayer(obj,LayerMask.NameToLayer(layer));}
    public static void SetLayer(GameObject obj,int layer) { SetLayer(obj.transform,layer);}
    public static void SetLayer(Transform obj,string layer) { SetLayer(obj,LayerMask.NameToLayer(layer));}
    public static void SetLayer(Transform obj,int layer)
    {
        obj.gameObject.layer = layer;
        foreach (Transform v in obj)
        {
            v.gameObject.layer = layer;
            if(v.childCount>0)
                SetLayer(v,layer);
        }
    }

    public Transform GetRoot(Transform trans, Type type)
    {
        if (trans.GetComponent(type) == null) return trans;
        Transform parent;
        while (true)
        {
            parent = trans.parent;
            if(parent==null)break;
            if (parent.GetComponent(type) != null)
                return parent;
        }
        return null;
    }

    public static List<Transform> GetChildren(Transform transform)
    {
        List<Transform> children = new List<Transform>();

        foreach (Transform v in transform)
        {
            children.Add(v);
            if(children.Count>0)
                children.AddRange(GetChildren(v));
        }
        
        return children;
    }
    
    public static Vector2 GetUISize(RectTransform rect)
    {
        List<Transform> childen = rect.transform.Cast<Transform>().ToList();

        Vector2 size = Vector2.zero;

        for (int i = 0; i < childen.Count; i++)
        {
            Transform c = childen[i];
            RectTransform rec = c.GetComponent<RectTransform>();
            if(rec==null) continue;
            
            Vector2 pivotSize = new Vector2(Mathf.Abs(rec.sizeDelta.x*(1F-rec.pivot.x)) , rec.sizeDelta.y*rec.pivot.y);
            Vector2 result = new Vector2(pivotSize.x+rec.anchoredPosition.x, pivotSize.y-rec.anchoredPosition.y);
            
            size = new Vector2(Mathf.Max(size.x, result.x), Mathf.Max(size.y, result.y));
        }
        
        return size;
    }
    
    public static bool ContainsLayer(LayerMask mask, int layer)
    {
        return (mask.value & (1 << layer)) != 0;
    }
    
    public static float NormalizeAngle(float angle)
    {
        while (angle > 180F) angle -= 360F;
        while (angle < -180F) angle += 360F;
        return angle;
    }

    public static float GetRotation(Transform t)
    {
        return NormalizeAngle(-t.eulerAngles.y);
    }
    public static float GetRotation(Quaternion q)
    {
        return NormalizeAngle(-q.eulerAngles.y);
    }
    public static float GetPitch(Transform t)
    {
        return NormalizeAngle(-t.eulerAngles.x);
    }
    public static float GetRoll(Transform t)
    {
        return NormalizeAngle(t.eulerAngles.z - 360F);
    }
}
