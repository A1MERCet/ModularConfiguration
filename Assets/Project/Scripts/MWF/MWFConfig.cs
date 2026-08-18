using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project;

[Serializable]
[JsonObject(MemberSerialization.OptIn)]
public abstract class MWFConfig
{
    public MWFPackage package;
    private JObject jsonObject;
    public JObject JsonObject => jsonObject;
    public Action<string, object> onPropertyChanged;
    public List<KVPair<string, object>> undoList = new();
    public List<KVPair<string, object>> redoList = new();
    
    public string path;
    
    [JsonProperty] private string internalName;
    public string InternalName
    {
        get => internalName;
        set
        {
            internalName = value;
            SetValue("internalName", value);
        }
    }
    
    
    [JsonProperty] private string displayName;
    public string DisplayName
    {
        get => displayName;
        set
        {
            displayName = value;
            SetValue("displayName", value);
        }
    }
    
    public void ParseJsonObject(JObject jsonObject)
    {
        this.jsonObject = jsonObject;
        OnParseJsonObject(jsonObject);
    }

    protected virtual void OnParseJsonObject(JObject jsonObject)
    {
    }

    public abstract string GetConfigType();

    public void SetValue(string key, object v)
    {
        PushUndo(new  KVPair<string, object>(key, JsonObject.GetVector3(key)));
        JsonObject.Set(key, v);
        OnPropertyChanged(key, v);
    }

    public virtual void OnPropertyChanged(string key, object value) => onPropertyChanged?.Invoke(key, value);

    public virtual void PushUndo(KVPair<string, object> item)
    {
        if (undoList.Count >= 200) undoList.RemoveAt(0);
        undoList.Add(item);
    }
    public virtual bool Undo()
    {
        if (undoList.Count == 0) return false;
        var item = undoList[^1];
        redoList.Add(new  KVPair<string, object>(item.Key, JsonObject.Get(item.Key)));
        JsonObject.Set(item.Key, item.Value);
        undoList.RemoveAt(undoList.Count - 1);
        return true;
    }
    public virtual bool Redo()
    {
        if (undoList.Count == 0) return false;
        var item = redoList[^1];
        undoList.Add(new  KVPair<string, object>(item.Key, JsonObject.Get(item.Key)));
        JsonObject.Set(item.Key, item.Value);
        redoList.RemoveAt(undoList.Count - 1);
        return true;
    }
}