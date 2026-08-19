using System.Collections.Generic;

public class BehaviourGLB: BehaviourMWF
{
    public MWFTypeRender ConfigTypeRender => Config as MWFTypeRender;
    public MWFRenderGLB RenderGLB => ConfigTypeRender?.configRender as MWFRenderGLB;
    
    
    private List<string> unnecessaryMode = new  List<string>()
    {
        "flashModel"
    };

    protected override void Start()
    {
        base.Start();
        ShowUnnecessaryModel();
    }
    
    public void HideInChildren(string name)
    {
        foreach (var c in UtilUnity.GetChildren(transform))
            if (c.name == name)
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