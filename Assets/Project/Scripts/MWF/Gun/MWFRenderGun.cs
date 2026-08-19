using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
[Serializable]
public class MWFRenderGun: MWFRenderGLB
{
    private SerializableDic<string, AttachmentPose> attachmentPoses = new();
    public SerializableDic<string, AttachmentPose> AttachmentPoses => attachmentPoses;
    
    /**
     * general
     */
    public bool basicSprint
    { 
        get => JsonObject["sprint"]?.GetBool("basicSprint", false) ?? false;
        set => SetValue("sprint.basicSprint", value);
    }
    public Vector3 sprintRotate
    {
        get => JsonObject["sprint"]?.GetVector3("sprintRotate") ?? Vector3.zero;
        set => SetValue("sprint.sprintRotate", value);
    }
    public Vector3 sprintTranslate
    {
        get => JsonObject["sprint"]?.GetVector3("sprintTranslate") ?? Vector3.zero;
        set => SetValue("sprint.sprintTranslate", value);
    }
    
    /**
     * aim
     */
    public Vector3 rotateHipPosition
    {
        get => JsonObject["aim"]?.GetVector3("rotateHipPosition") ?? Vector3.zero;
        set => SetValue("aim.rotateHipPosition", value);
    }
    public Vector3 translateHipPosition
    {
        get => JsonObject["aim"]?.GetVector3("translateHipPosition") ?? Vector3.zero;
        set => SetValue("aim.translateHipPosition", value);
    }
    public Vector3 rotateAimPosition
    {
        get => JsonObject["aim"]?.GetVector3("rotateAimPosition") ?? Vector3.zero;
        set => SetValue("aim.rotateAimPosition", value);
    }
    public Vector3 translateAimPosition
    {
        get => JsonObject["aim"]?.GetVector3("translateAimPosition") ?? Vector3.zero;
        set => SetValue("aim.translateAimPosition", value);
    }
    
    /**
     * global
     */
    public Vector3 globalScale
    {
        get => JsonObject["global"]?.GetVector3("globalScale") ?? Vector3.zero;
        set => SetValue("global.globalScale", value);
    }
    public Vector3 globalTranslate
    {
        get => JsonObject["global"]?.GetVector3("globalTranslate") ?? Vector3.zero;
        set => SetValue("global.globalTranslate", value);
    }
    public Vector3 globalRotate
    {
        get => JsonObject["global"]?.GetVector3("globalRotate") ?? Vector3.zero;
        set => SetValue("global.globalRotate", value);
    }
    
    /**
     * extra
     */
    public float modelScale
    { 
        get => JsonObject["extra"]?.GetFloat("modelScale", 30F) ?? 30F;
        set => SetValue("extra.modelScale", value);
    }
    
    public override string GetConfigType() => "guns";

    public override void OnPropertyChanged(string key, object value)
    {
        base.OnPropertyChanged(key, value);
        if (key.StartsWith("attachments")) ParseAttachmentPoses();
    }
    
    protected override void OnParseJsonObject(JObject jsonObject)
    {
        base.OnParseJsonObject(jsonObject);
        ParseAttachmentPoses();
    }

    protected virtual void ParseAttachmentPoses()
    {
        
    }
    
    public override BehaviourMWF LoadedModel() => loadedGLBScene?.GetComponent<BehaviourMWFGun>();
    protected override void OnGLBLoaded(GLBScene scene)
    {
        base.OnGLBLoaded(scene);
        var behaviour = scene.gameObject.AddComponent<BehaviourMWFGun>();
        behaviour.SetConfig(configType, scene.gameObject);
    }
}