using Project;

public class MWFTypeAtt: MWFTypeOBJ
{
    public MWFRenderAtt RenderAtt => configRender as MWFRenderAtt;
    
    public string slot
    {
        get => JsonObject.Get<string>("slot");
        set => SetValue("Slot", value);
    }
    
    public override string GetConfigType() => "attachments";
}