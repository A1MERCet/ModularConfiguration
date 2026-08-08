using System;
using System.Collections.Generic;
using System.IO;

public class MWFPackage
{
    public string name;
    public string path;
    public string assetsPath;
    public string glbPath;
    
    private List<MWFType> types = new();
    private List<MWFRender> renders = new();
    
    public List<MWFType> Types => types;
    public List<MWFRender> Renders => renders;

    public T GetMWFType<T>(string id) where T : MWFType
    {
        foreach (MWFType t in types)
            if (t.internalName == id && t.GetType() == typeof(T))
                return t as T;
        return null;
    }
    
    public T GetMWFRender<T>(string id) where T : MWFRender
    {
        foreach (MWFRender r in renders)
            if (r.internalName == id && r.GetType() == typeof(T))
                return r as T;
        return null;
    }

    public void LoadGLB(MWFRender render, Action<GLBScene> onLoaded = null)
    {
        GLBSceneManager.instance.Load(Path.Combine(glbPath, render.GetRenderType()), render.modelFileName, (s) => {
            render.loadedGLBScene = s;
            onLoaded?.Invoke(s);
        });
    }
}