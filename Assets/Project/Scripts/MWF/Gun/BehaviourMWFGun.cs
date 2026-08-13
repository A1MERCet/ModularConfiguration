using System;
using UnityEngine;

public class BehaviourMWFGun: MonoBehaviour
{
    private MWFTypeGun type;
    private MWFRenderGun render;
    private GLBScene glbScene;

    public MWFTypeGun Type => type;
    public MWFRenderGun Render => render;
    public GLBScene GLBScene => glbScene;

    private bool aiming = false;


    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) aiming = !aiming;
    }
    
    private void FixedUpdate()
    {
        if (render != null && glbScene != null)
        {
            var targetPosition = (render.translateHipPosition() + render.globalTranslate());
            if (aiming) targetPosition += render.translateAimPosition();
            
            var targetRotation = render.rotateHipPosition() + render.globalRotate() ;
            // glbScene.transform.localScale = render.globalScale() * render.modelScale();
            glbScene.transform.position = new Vector3(targetPosition.z, targetPosition.y, targetPosition.x) * 0.01F + new Vector3(0,2,0);
            glbScene.transform.rotation = Quaternion.identity;
            glbScene.transform.Rotate(Vector3.up, 90F);
            // glbScene.transform.rotation = Quaternion.Euler(targetRotation+ new Vector3(0,90,0));
        }
    }
    
    public void SetConfig(MWFTypeGun type, MWFRenderGun render)
    {
        this.type = type;
        this.render = render;
    }

    public void SetGLBScene(GLBScene glbScene)
    {
        this.glbScene = glbScene;
    }
}