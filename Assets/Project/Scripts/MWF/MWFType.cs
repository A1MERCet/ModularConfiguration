using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
[Serializable]
public abstract class MWFType: MWFConfig
{
    public Action<Texture2D> onIconLoaded;
}