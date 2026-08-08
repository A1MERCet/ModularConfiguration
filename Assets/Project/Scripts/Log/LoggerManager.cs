using System;
using UnityEngine;

public class LoggerManager : SingletonMono<LoggerManager>
{
    public void Info(params object[] msg)
    {
        string str = "[信息]";
        foreach (var o in msg)
            str += BuildObjectString(o);
        Debug.Log(str);
    }
    
    public void Warn(params string[] msg)
    {
        string str = "[警告]";
        foreach (var o in msg)
            str += BuildObjectString(o);
        Debug.LogWarning(str);
    }
    
    public void Error(params string[] msg)
    {
        string str = "[错误]";
        foreach (var o in msg)
            str += BuildObjectString(o);
        Debug.LogError(str);
    }

    private string BuildObjectString(object o)
    {
        if (o == null) return " null";
        try {
            return $"{ o.ToString()}";
        }catch (Exception e) {
            Debug.LogError(e);
            return " null";
        }
    }
}