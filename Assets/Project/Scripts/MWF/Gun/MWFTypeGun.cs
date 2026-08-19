using System;
using Newtonsoft.Json;
using Project;

[JsonObject(MemberSerialization.OptIn)]
[Serializable]
public class MWFTypeGun: MWFTypeGLB
{
    public MWFRenderGun RenderGun => configRender as MWFRenderGun;


    public string[] acceptedAmmo
    {
        get => JsonObject.Get<string[]>("acceptedAmmo");
        set => JsonObject.Set<string[]>("acceptedAmmo", value);
    }
    
    
    public override string GetConfigType() => "guns";
}
