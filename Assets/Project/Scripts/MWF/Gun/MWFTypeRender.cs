using System;
using Newtonsoft.Json;
using Project;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
[Serializable]
public abstract class MWFTypeRender : MWFType
{
    public MWFRender configRender;
    public ModelSkin[] modelSkins
    {
        get => JsonObject.Get<ModelSkin[]>("modelSkins");
        set => JsonObject.Set<ModelSkin[]>("modelSkins", value);
    }
    public override void OnPropertyChanged(string key, object value)
    {
        base.OnPropertyChanged(key, value);
        if (key == "internalName" && configRender != null) configRender.InternalName = key;
    }
}