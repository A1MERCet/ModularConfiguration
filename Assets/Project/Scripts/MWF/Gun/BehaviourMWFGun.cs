using System;
using System.Collections.Generic;
using Project;
using UnityEngine;

public class BehaviourMWFGun: BehaviourGLB
{
    public MWFTypeGun ConfigGun => Config as MWFTypeGun;
    public MWFRenderGun RenderGun => ConfigGun?.configRender as MWFRenderGun;
    private bool aiming = false;
    private bool spring = false;
    public Dictionary<string, BehaviourMWFAtt> attachments = new();
    public Dictionary<string, Transform> attachPositions = new();
    
    private IncrementPos.Pos targetAimPos = new();
    private IncrementPos.Pos targetSprintPos = new();

    public void RemoveAttach(string attachType) {
        if (attachments.ContainsKey(attachType))
        {
            var attachGroup = RenderGun.attachmentGroup;
            attachGroup.TryGetValue(attachType, out MWFRenderGun.AttachSlot slot);
            if (slot != null) ShowInChildren(slot.hidePart);
            
            Destroy(attachments[attachType].gameObject);
            attachments.Remove(attachType);
        }
    }
    
    public void AddAttachment(MWFTypeAtt att)
    {
        RemoveAttach(att.attachmentType);
        var behaviour = att.RenderAtt.LoadOBJ();
        behaviour.transform.parent = attachPositions.ContainsKey(att.attachmentType) ? attachPositions[att.attachmentType] : model.transform;
        attachments.Add(att.attachmentType, behaviour as BehaviourMWFAtt);
        var attachGroup = RenderGun.attachmentGroup;
        attachGroup.TryGetValue(att.attachmentType, out MWFRenderGun.AttachSlot slot);
        if (slot != null) HideInChildren(slot.hidePart);
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
        HideInChildren(RenderGun?.defaultHidePart ?? Array.Empty<string>());
    }

    void UpdateAttachPos()
    {
        var attachGroup = RenderGun.attachmentGroup;
        foreach (var kv in attachGroup) {
            var attachType = kv.Key;
            if (attachPositions.ContainsKey(attachType)) continue;
            GameObject node = new GameObject(attachType);
            node.transform.SetParent(GetNodeOrDefault("gunModel"));
            node.transform.localPosition = Vector3.zero;
            node.transform.localRotation = Quaternion.identity;
            attachPositions.Add(attachType, node.transform);
        }

        foreach (var kv in attachGroup)
        {
            var attachType = kv.Key;
            var attachSlot = kv.Value;
            var bindPart = GetNodeOrDefault("gunModel");
            Transform node = attachPositions[attachType];
            node.transform.SetParent(bindPart.transform, false);
            
            Vector3 localPos = attachSlot.translate.ToVector3();
            Quaternion rot = Quaternion.AngleAxis(-180F, Vector3.up);
            node.transform.localPosition = rot * localPos;
            node.transform.localRotation = rot * Quaternion.Euler(attachSlot.Rotate);
        }
    }
    
    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(1) && !UIConfigManger.instance.Editing) {
            aiming = !aiming;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !UIConfigManger.instance.Editing) {
            spring = !spring;
        }
        if (UIConfigManger.instance.Editing) {
            aiming = false;
            spring = false;
        }
        
        if (RenderGun != null && model != null) {
            targetAimPos.position = Vector3.Lerp(targetAimPos.position, aiming ? RenderGun.translateAimPosition: Vector3.zero, Time.deltaTime * 20F);
            targetAimPos.rotation = Vector3.Lerp(targetAimPos.rotation, aiming ? RenderGun.rotateAimPosition: Vector3.zero, Time.deltaTime * 20F);
            
            targetSprintPos.position = Vector3.Lerp(targetSprintPos.position, spring ? RenderGun.sprintTranslate: Vector3.zero, Time.deltaTime * 20F);
            targetSprintPos.rotation = Vector3.Lerp(targetSprintPos.rotation, spring ? RenderGun.sprintRotate: Vector3.zero, Time.deltaTime * 20F);
            
            incrementPos["gun"].position = UtilMC.Location2Vector(RenderGun.translateHipPosition + RenderGun.globalTranslate + targetAimPos.position + targetSprintPos.position);
            incrementPos["gun"].rotation = UtilMC.Rotation2Vector(RenderGun.rotateHipPosition + RenderGun.globalRotate + targetAimPos.rotation + targetSprintPos.rotation) + new Vector3(0, 90F, 0F);
            // incrementPos["gun"].position = UtilMC.Location2Vector(spring ? targetSprintPos.position : (RenderGun.translateHipPosition + RenderGun.globalTranslate + targetAimPos.position));
            // incrementPos["gun"].rotation = UtilMC.Rotation2Vector(spring ? targetSprintPos.rotation : (RenderGun.rotateHipPosition + RenderGun.globalRotate + targetAimPos.rotation)) + new Vector3(0, 90F, 0F);
        }
        
        UpdateAttachPos();
        
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (GetNode("leftArmModel")?.gameObject.activeInHierarchy ?? false) {
                HideInChildren(new string[] {
                    "leftArmModel",
                    "leftArmLayerModel",
                    "leftArmSlimModel",
                    "leftArmLayerSlimModel",
                    "rightArmModel",
                    "rightArmLayerModel",
                    "rightArmSlimModel",
                    "rightArmLayerSlimModel",
                });
            }else {
                ShowInChildren(new string[] {
                    "leftArmModel",
                    "leftArmLayerModel",
                    "leftArmSlimModel",
                    "leftArmLayerSlimModel",
                    "rightArmModel",
                    "rightArmLayerModel",
                    "rightArmSlimModel",
                    "rightArmLayerSlimModel",
                });
            }
      
        }
    }
}