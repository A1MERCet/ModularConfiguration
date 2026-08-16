using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class MWFTypeGun: MWFType
{
    public override string GetConfigType() => "guns";
    public MWFRenderGun renderGun;
}
