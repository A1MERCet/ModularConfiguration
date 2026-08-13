using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Project;
using UnityEngine;

public class MWFRenderGun: MWFRender
{
    private Dictionary<string, AnimationStage> animations = new();
    public Dictionary<string, AnimationStage> Animations => animations;

    /**
     * aim
     */
    public Vector3 rotateHipPosition() => JsonObject["aim"]?.GetVector3("rotateHipPosition") ?? Vector3.zero;
    public void rotateHipPosition(Vector3 v) => JsonObject["aim"]?.SetVector3("rotateHipPosition", v);
    public Vector3 translateHipPosition() => JsonObject["aim"]?.GetVector3("translateHipPosition") ?? Vector3.zero;
    public void translateHipPosition(Vector3 v) => JsonObject["aim"]?.SetVector3("translateHipPosition", v);
    public Vector3 rotateAimPosition() => JsonObject["aim"]?.GetVector3("rotateAimPosition") ?? Vector3.zero;
    public void rotateAimPosition(Vector3 v) => JsonObject["aim"]?.SetVector3("rotateAimPosition", v);
    public Vector3 translateAimPosition() => JsonObject["aim"]?.GetVector3("translateAimPosition") ?? Vector3.zero;
    public void translateAimPosition(Vector3 v) => JsonObject["aim"]?.SetVector3("translateAimPosition", v);
    /**
     * global
     */
    public Vector3 globalScale() => JsonObject["global"]?.GetVector3("globalScale", Vector3.one) ?? Vector3.one;
    public void globalScale(Vector3 v) => JsonObject["global"]?.SetVector3("globalScale", v);
    public Vector3 globalTranslate() => JsonObject["global"]?.GetVector3("globalTranslate") ?? Vector3.zero;
    public void globalTranslate(Vector3 v) => JsonObject["global"]?.SetVector3("globalTranslate", v);
    public Vector3 globalRotate() => JsonObject["global"]?.GetVector3("globalRotate") ?? Vector3.zero;
    public void globalRotate(Vector3 v) => JsonObject["global"]?.SetVector3("globalRotate", v);
    /**
     * extra
     */
    public float modelScale() => JsonObject["extra"]?.GetFloat("modelScale", 1F) ?? 1F;
    public void modelScale(float v) => JsonObject["extra"]?.SetFloat("modelScale", v);
    
    protected override void OnParseJsonObject(JObject jsonObject)
    {
        base.OnParseJsonObject(jsonObject);
        ParseAnimationStages();
    }

    public override string GetRenderType() => "guns";

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