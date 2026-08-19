using System.Collections.Generic;
using Project;
using UnityEngine;

public class BehaviourMWFGun: BehaviourGLB
{
    public MWFTypeGun ConfigGun => Config as MWFTypeGun;
    public MWFRenderGun RenderGun => ConfigGun?.configRender as MWFRenderGun;
    private bool aiming = false;
    public Dictionary<string, List<BehaviourMWF>> attachments = new();
    
    private IncrementPos.Pos targetAimPos = new();

    public void RemoveAttachment(MWFTypeAtt att)
    {
        if (attachments.ContainsKey(att.slot))
            foreach (var behaviour in new List<BehaviourMWF>(attachments[att.slot]))
                if (behaviour.Config == null || behaviour.Config.InternalName == att.InternalName)
                {
                    attachments[att.slot].Remove(behaviour);
                    Destroy(behaviour.gameObject);
                }
    }
    
    public void AddAttachment(MWFTypeAtt att)
    {
        RemoveAttachment(att);
        var behaviour = att.RenderAtt.LoadOBJ();
        if (!attachments.ContainsKey(att.InternalName)) attachments.Add(att.InternalName, new List<BehaviourMWF>());
        attachments[att.InternalName].Add(behaviour);
    }

    protected override void Awake()
    {
        base.Awake();
        incrementPos.Add("gun");
    }
    
    protected override void Start()
    {
        base.Start();
        HideInChildren("flashModel");
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(1) && !UIConfigManger.instance.Editing) aiming = !aiming;
        if (aiming && UIConfigManger.instance.Editing) aiming = false;
        
        if (RenderGun != null && model != null)
        {
            targetAimPos.position = Vector3.Lerp(targetAimPos.position, aiming ? RenderGun.translateAimPosition: Vector3.zero, Time.deltaTime * 20F);
            targetAimPos.rotation = Vector3.Lerp(targetAimPos.rotation, aiming ? RenderGun.rotateAimPosition: Vector3.zero, Time.deltaTime * 20F);
            
            incrementPos["gun"].position = UtilMC.Location2Vector(RenderGun.translateHipPosition + RenderGun.globalTranslate + targetAimPos.position);
            incrementPos["gun"].rotation = UtilMC.Rotation2Vector(RenderGun.rotateHipPosition + RenderGun.globalRotate + targetAimPos.rotation) + new Vector3(0, 90F, 0F);
        }
    }
}