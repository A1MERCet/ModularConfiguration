using Project;

public class MWFTypeAtt: MWFTypeOBJ
{
    public MWFRenderAtt RenderAtt => configRender as MWFRenderAtt;
    
    public string attachmentType
    {
        get => JsonObject.Get<string>("attachmentType");
        set => SetValue("attachmentType", value);
    }
    
    public override string GetConfigType() => "attachments";
}