using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class MWFRenderGun: MWFRender
{
    private Dictionary<string, AnimationStage> animations = new();
    public Dictionary<string, AnimationStage> Animations => animations;

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