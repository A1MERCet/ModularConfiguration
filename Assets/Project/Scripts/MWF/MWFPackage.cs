using System;
using System.Collections.Generic;
using System.IO;

public class MWFPackage
{
    public LoggerProxy logger = new LoggerProxy("MWF包管理器");
    
    public string name;
    public string path;
    public string assetsPath;
    public string skinPath;
    public string iconPath;
    public string objPath;
    public string glbPath;
    
    private SerializableDic<string, MWFConfig> configs = new();
    public SerializableDic<string, MWFConfig> Configs => configs;

    public T GetConfig<T>(string id) where T : MWFConfig
    {
        foreach (MWFConfig r in configs.Values)
            if (r.GetType() == typeof(T) && r.InternalName == id)
                return (T)r;
        return null;
    }
    
    public MWFConfig GetConfig(string id)
    {
        foreach (MWFConfig r in configs.Values)
            if (r.InternalName == id)
                return r;
        return null;
    }
    
    public List<T> GetList<T>() where T : MWFConfig
    {
        List<T> list = new();
        foreach (MWFConfig r in configs.Values)
            if (r.GetType() == typeof(T))
                list.Add(r as T);
        return list;
    }

    public T RemoveMWFRender<T>(string id) where T : MWFConfig
    {
        var cfg = configs[id];
        if (cfg.GetType() == typeof(T))
        {
            var c = cfg as T;
            configs.Remove(id);
            return c;
        }
        return null;
    }
    
    public MWFConfig RemoveMWFRender(string id)
    {
        var cfg = configs[id];
        if (cfg != null)
        {
            configs.Remove(id);
            return cfg;
        }
        return null;
    }

    public void AddConfig(MWFConfig config)
    {
        if (config == null){logger.Error("添加Config失败 - Config为null");return;}
        if (config.InternalName == null){logger.Error("添加Config失败 - InternalName为空");return;}
        if (configs.ContainsKey(config.InternalName)){logger.Error($"添加Config({config.InternalName})失败 - 重复的InternalName");return;}
        configs.Add(config.InternalName, config);
    }
}