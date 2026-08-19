using System.Collections.Generic;

public class BehaviourGLB: BehaviourMWF
{
    public MWFTypeRender ConfigTypeRender => Config as MWFTypeRender;
    public MWFRenderGLB RenderGLB => ConfigTypeRender?.configRender as MWFRenderGLB;
    
    
    private List<string> unnecessaryMode = new()
    {
        "flashModel"
    };

    protected override void Start()
    {
        base.Start();
        ShowUnnecessaryModel();
    }
    
    public void HideInChildrenStartsWith(string name)
    {
        foreach (var c in UtilUnity.GetChildren(transform))
            if (c.name.StartsWith(name))
                c.gameObject.SetActive(false);
    }
    
    public void HideInChildrenEndsWith(string name)
    {
        foreach (var c in UtilUnity.GetChildren(transform))
            if (c.name.EndsWith(name))
                c.gameObject.SetActive(false);
    }
    
    public void HideInChildren(string name)
    {
        foreach (var c in UtilUnity.GetChildren(transform))
            if (c.name == name)
                c.gameObject.SetActive(false);
    }
    
    public void HideInChildren(string[] name)
    {
        foreach (var c in UtilUnity.GetChildren(transform))
            for (var i = 0; i < name.Length; i++)
                if (c.name == name[i])
                {
                    c.gameObject.SetActive(false);
                    continue;
                }
    }
    
    public void HideInChildren(List<string> name)
    {
        foreach (var c in UtilUnity.GetChildren(transform))
            if (name.Contains(c.name))
                c.gameObject.SetActive(false);
    }
    
    public void HideUnnecessaryModel()
    {
        foreach (var c in UtilUnity.GetChildren(transform))
            if (unnecessaryMode.Contains(c.name))
                c.gameObject.SetActive(false);
    }
    public void ShowUnnecessaryModel()
    {
        foreach (var c in UtilUnity.GetChildren(transform))
            if (unnecessaryMode.Contains(c.name))
                c.gameObject.SetActive(true);
    }
}