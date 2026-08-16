using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
public class MWFRenderGun: MWFRender
{
    private Dictionary<string, AnimationStage> animations = new();
    public Dictionary<string, AnimationStage> Animations => animations;

    
    /**
     * general
     */
    public float FPS
    { 
        get => JsonObject?.GetFloat("FPS", 30F) ?? 30F;
        set {
            JsonObject?.SetFloat("FPS", value);
            onPropertyChanged("FPS", value);
        }
    }
    /**
     * aim
     */
    public Vector3 rotateHipPosition
    {
        get => JsonObject["aim"]?.GetVector3("rotateHipPosition") ?? Vector3.zero;
        set {
            JsonObject["aim"]?.SetVector3("rotateHipPosition", value);
            onPropertyChanged("aim.rotateHipPosition", value);
        }
    }
    public Vector3 translateHipPosition
    {
        get => JsonObject["aim"]?.GetVector3("translateHipPosition") ?? Vector3.zero;
        set {
            JsonObject["aim"]?.SetVector3("translateHipPosition", value);
            onPropertyChanged("aim.translateHipPosition", value);
        }
    }
    public Vector3 rotateAimPosition
    {
        get => JsonObject["aim"]?.GetVector3("rotateAimPosition") ?? Vector3.zero;
        set {
            JsonObject["aim"]?.SetVector3("rotateAimPosition", value);
            onPropertyChanged("aim.rotateAimPosition", value);
        }
    }
    public Vector3 translateAimPosition
    {
        get => JsonObject["aim"]?.GetVector3("translateAimPosition") ?? Vector3.zero;
        set {
            JsonObject["aim"]?.SetVector3("translateAimPosition", value);
            onPropertyChanged("aim.translateAimPosition", value);
        }
    }
    /**
     * global
     */
    public Vector3 globalScale
    {
        get => JsonObject["global"]?.GetVector3("globalScale") ?? Vector3.zero;
        set {
            JsonObject["global"]?.SetVector3("globalScale", value);
            onPropertyChanged("global.globalScale", value);
        }
    }
    public Vector3 globalTranslate
    {
        get => JsonObject["global"]?.GetVector3("globalTranslate") ?? Vector3.zero;
        set {
            JsonObject["global"]?.SetVector3("globalTranslate", value);
            onPropertyChanged("global.globalTranslate", value);
        }
    }
    public Vector3 globalRotate
    {
        get => JsonObject["global"]?.GetVector3("globalRotate") ?? Vector3.zero;
        set {
            JsonObject["global"]?.SetVector3("globalRotate", value);
            onPropertyChanged("global.globalRotate", value);
        }
    }
    /**
     * extra
     */
    public float modelScale
    { 
        get => JsonObject["extra"]?.GetFloat("modelScale", 30F) ?? 30F;
        set {
            JsonObject["extra"]?.SetFloat("modelScale", value);
            onPropertyChanged("extra.modelScale", value);
        }
    }
    
    protected override void OnParseJsonObject(JObject jsonObject)
    {
        base.OnParseJsonObject(jsonObject);
        ParseAnimationStages();
    }

    public override string GetConfigType() => "guns";

    private void ParseAnimationStages()
    {
        this.animations.Clear();
        JObject animations = JsonObject["animations"] as JObject;
        if (animations == null) return;
        
        foreach (var prop in animations.Properties())
        {
            JObject anim = (JObject)prop.Value;
            this.animations[prop.Name] = new AnimationStage {
                name = prop.Name,
                startTime = anim["startTime"]?.Value<int>() ?? 0,
                endTime   = anim["endTime"]?.Value<int>() ?? 0,
                speed     = anim["speed"]?.Value<float>() ?? 0F
            };
        }
    }
}