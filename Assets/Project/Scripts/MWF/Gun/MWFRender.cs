using Newtonsoft.Json.Linq;
using UnityEngine;

public abstract class MWFRender
{
    public string path;
    private JObject jsonObject;
    public JObject JsonObject => jsonObject;
    
    public string internalName;
    public string modelFileName;

    public GLBScene loadedGLBScene;

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