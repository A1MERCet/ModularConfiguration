using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Project;

[JsonObject(MemberSerialization.OptIn)]
[Serializable]
public class MWFTypeGun: MWFTypeGLB
{
    public struct WeaponSound
    {
        public string soundEvent;
        public string soundName;
        public string soundNameDistant;
        public float soundRange;
        public float soundMaxRange;
        public float soundFadeMultiplier;
    }
    
    public MWFRenderGun RenderGun => configRender as MWFRenderGun;

    public string[] fireModes
    {
        get => JsonObject.Get<string[]>("fireModes");
        set => SetValue("fireModes", value);
    }
    
    public string[] acceptedAmmo
    {
        get => JsonObject.Get<string[]>("acceptedAmmo");
        set => SetValue("acceptedAmmo", value);
    }
    
    public Dictionary<string, WeaponSound> weaponSoundMap
    {
        get => JsonObject.Get<Dictionary<string, WeaponSound>>("weaponSoundMap");
        set => SetValue("weaponSoundMap", value);
    }
    
    public string[] AcceptedAttachments(string id) => JsonObject.Get<string[]>($"acceptedAttachments.{id}") ?? Array.Empty<string>();
    public void AcceptedAttachments(string id, string[] ary) => SetValue($"acceptedAttachments.{id}", ary);
    
    public Dictionary<string, string[]> acceptedAttachments
    {
        get => JsonObject.Get<Dictionary<string, string[]>>("acceptedAttachments");
        set => SetValue("acceptedAttachments", value);
    }
    
    public override string GetConfigType() => "guns";
}
