using System.Collections.Generic;
using UnityEngine;

public class BehaviourMWFGun: BehaviourMWF
{
    public MWFTypeGun ConfigGun => Config as MWFTypeGun;
    public MWFRenderGun ConfigRender => ConfigGun.configRender as MWFRenderGun;
    private bool aiming = false;
    public Dictionary<string, List<BehaviourMWF>> attachments = new();

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

    protected override void Start()
    {
        base.Start();
        ShowUnnecessaryModel();
        HideInChildren("flashModel");
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(1)) aiming = !aiming;
    }
    
    protected override  void FixedUpdate()
    {
        base.FixedUpdate();
        if (ConfigRender != null && model != null)
        {
            var targetPosition = (ConfigRender.translateHipPosition + ConfigRender.globalTranslate);
            if (aiming) targetPosition += ConfigRender.translateAimPosition;
            model.transform.localPosition = new Vector3(targetPosition.z, targetPosition.y, targetPosition.x) * 0.01F + new Vector3(0,2,0);
            model.transform.localRotation = Quaternion.identity;
            model.transform.Rotate(Vector3.up, 90F);
        }
    }

    private List<string> unnecessaryMode = new  List<string>()
    {
        "flashModel"
    };

    public void HideInChildren(string name)
    {
        foreach (var c in UtilUnity.GetChildren(transform))
            if (c.name == name)
                c.gameObject.SetActive(false);
    }
    
    public void HideUnnecessaryModel()
    {
        foreach (var c in UtilUnity.GetChildren(transform))
            if (unnecessaryMode.Contains(c.name))
                c.gameObject.SetActive(false);
    }
    public void ShowUnnecessaryModel()
    {
        foreach (var c in UtilUnity.GetChildren(transform))
            if (unnecessaryMode.Contains(c.name))
                c.gameObject.SetActive(true);
    }
}