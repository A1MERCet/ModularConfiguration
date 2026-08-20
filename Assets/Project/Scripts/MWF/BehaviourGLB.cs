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
    
    public void ShowUnnecessaryModel()
    {
        foreach (var c in UtilUnity.GetChildren(transform))
            if (unnecessaryMode.Contains(c.name))
                c.gameObject.SetActive(true);
    }
    
    public void HideUnnecessaryModel()
    {
        foreach (var c in UtilUnity.GetChildren(transform))
            if (unnecessaryMode.Contains(c.name))
                c.gameObject.SetActive(false);
    }
}