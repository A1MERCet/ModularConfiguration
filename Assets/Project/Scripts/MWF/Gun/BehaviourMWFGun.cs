using System;
using UnityEngine;

public class BehaviourMWFGun: BehaviourMWF
{
    private MWFTypeGun type;

    public MWFTypeGun TypeGun => type;
    public MWFRenderGun RenderGun => type.renderGun;

    private bool aiming = false;


    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(1)) aiming = !aiming;
    }
    
    protected override  void FixedUpdate()
    {
        base.FixedUpdate();
        if (RenderGun != null && GLBScene != null)
        {
            var targetPosition = (RenderGun.translateHipPosition + RenderGun.globalTranslate);
            if (aiming) targetPosition += RenderGun.translateAimPosition;
            GLBScene.transform.position = new Vector3(targetPosition.z, targetPosition.y, targetPosition.x) * 0.01F + new Vector3(0,2,0);
            GLBScene.transform.rotation = Quaternion.identity;
            GLBScene.transform.Rotate(Vector3.up, 90F);
        }
    }
    
    public virtual void SetConfig(MWFType type, GLBScene glbScene)
    {
        base.SetConfig(type, glbScene);
        this.type = type as MWFTypeGun;
    }
}