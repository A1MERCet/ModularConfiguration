using System;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
[Serializable]
public class MWFTypeGun: MWFTypeGLB
{
    public MWFRenderGLB RenderGun => configRender as MWFRenderGLB;
    
    public override string GetConfigType() => "guns";
}
