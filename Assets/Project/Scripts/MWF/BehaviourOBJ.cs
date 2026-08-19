public class BehaviourOBJ: BehaviourMWF
{
    public MWFTypeRender ConfigTypeRender => Config as MWFTypeRender;
    public MWFRenderOBJ RenderOBJ => ConfigTypeRender?.configRender as MWFRenderOBJ;
}