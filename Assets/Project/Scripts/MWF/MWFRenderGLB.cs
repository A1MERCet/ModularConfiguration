using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

public abstract class MWFRenderGLB: MWFRender
{
    private Dictionary<string, GLBAnimationStage> animations = new();
    public Dictionary<string, GLBAnimationStage> Animations => animations;

    public GLBScene loadedGLBScene;

    public virtual void LoadGLB(Action<GLBScene> onLoaded)
    {
        TexturePBR texture = new TexturePBR() {
            baseColorPath = configType.modelSkins.Length == 0 ? "" : Path.Combine(configType.package.skinPath, configType.GetConfigType(), $"{configType.modelSkins[0].skinAsset}.png")
        };
        GLBSceneManager.instance.Load(Path.Combine(configType.package.glbPath, configType.GetConfigType()), ModelFileName, texture, (scene) => {
            OnGLBLoaded(scene);
            onLoaded?.Invoke(scene);
        });
    }

    public override void OnPropertyChanged(string key, object value)
    {
        base.OnPropertyChanged(key, value);
        if (key.StartsWith("animations")) ParseAnimationStages();
    }

    protected override void OnParseJsonObject(JObject jsonObject)
    {
        base.OnParseJsonObject(jsonObject);
        ParseAnimationStages();
    }
    
    private void ParseAnimationStages()
    {
        Animations.Clear();
        JObject animations = JsonObject["animations"] as JObject;
        if (animations == null) return;
        
        foreach (var prop in animations.Properties())
        {
            JObject anim = (JObject)prop.Value;
            Animations[prop.Name] = new GLBAnimationStage {
                name = prop.Name,
                startTime = anim["startTime"]?.Value<int>() ?? 0,
                endTime   = anim["endTime"]?.Value<int>() ?? 0,
                speed     = anim["speed"]?.Value<float>() ?? 0F
            };
        }
    }
    
    protected virtual void OnGLBLoaded(GLBScene scene)
    {
        
    }
}