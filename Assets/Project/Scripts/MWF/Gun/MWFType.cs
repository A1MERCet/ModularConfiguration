using Newtonsoft.Json.Linq;

public abstract class MWFType
{
    public string path;
    private JObject jsonObject;
    public JObject JsonObject => jsonObject;

    public string internalName;
    public string displayName;
    
    public void ParseJsonObject(JObject jsonObject)
    {
        this.jsonObject = jsonObject;
        OnParseJsonObject(jsonObject);
    }

    protected virtual void OnParseJsonObject(JObject jsonObject)
    {
        
    }
    
    public abstract string GetRenderType();
}