using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
public abstract class MWFRender: MWFConfig
{
    [JsonProperty] private string modelFileName;
    public string ModelFileName
    {
        get => modelFileName;
        set {
            modelFileName = value;
            OnPropertyChanged("modelFileName", value);
        }
    }

    public GLBScene loadedGLBScene;
}