using System;
using Newtonsoft.Json;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
[Serializable]
public abstract class MWFRender: MWFConfig
{
    public MWFTypeRender configType;
    public ModelRenderer.RenderParameters renderParams;
    
    [JsonProperty] private string modelFileName;
    public string ModelFileName
    {
        get => modelFileName;
        set => SetValue("modelFileName", value);
    }

    public abstract BehaviourMWF LoadedModel();
}