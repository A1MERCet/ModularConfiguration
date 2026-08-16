using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public abstract class MWFConfig
{
    public MWFPackage package;
    private JObject jsonObject;
    public JObject JsonObject => jsonObject;
    public Action<string, object> onPropertyChanged;
    
    public string path;
    
    [JsonProperty] private string internalName;
    public string InternalName
    {
        get => internalName;
        set {
            internalName = value;
            OnPropertyChanged("internalName", value);
        }
    }
    
    
    [JsonProperty] private string displayName;
    public string DisplayName
    {
        get => displayName;
        set {
            displayName = value;
            OnPropertyChanged("displayName", value);
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
    
    public virtual void OnPropertyChanged(string key, object value) => onPropertyChanged?.Invoke(key, value);
}