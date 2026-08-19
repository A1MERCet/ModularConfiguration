
using Project;
using UnityEngine;

public class UIConfigPropertyGun : UIConfigProperty
{
    public MWFTypeGun ConfigGun => type as MWFTypeGun;
    public MWFRenderGun RenderGun => ConfigGun?.RenderGun;

    public void EditHipPos()
    {
        UIConfigManger.instance.EditBehaviourMWF.updatePos = false;
        var defPos = new IncrementPos.Pos(UtilMC.Location2Vector(RenderGun.translateHipPosition), UtilMC.Rotation2Vector(RenderGun.rotateHipPosition));
        var editProperty = new UIConfigManger.EditProperty()
        {
            defPos = defPos,
            onEditApply = (pos) => {
                RenderGun.translateHipPosition = new Vector3(pos.position.z, pos.position.y, pos.position.x) * 100F;
                RenderGun.rotateHipPosition = pos.rotation;
                UIConfigManger.instance.EditBehaviourMWF.updatePos = true;
            }, 
            onEditUpdate = (pos) => {
                UIConfigManger.instance.EditBehaviourMWF.incrementPosModify["showcase"].position = pos.position;
                UIConfigManger.instance.EditBehaviourMWF.incrementPosModify["showcase"].rotation = pos.rotation;
            }, 
            onEditCancel = (pos) => {
                UIConfigManger.instance.EditBehaviourMWF.incrementPosModify["showcase"].position = Vector3.zero;
                UIConfigManger.instance.EditBehaviourMWF.incrementPosModify["showcase"].rotation = Vector3.zero;
                UIConfigManger.instance.EditBehaviourMWF.updatePos = true;
            }
        };
        UIConfigManger.instance.StartPosEdit(editProperty);
    }
    
    public void EditAimPos()
    {
        UIConfigManger.instance.StartPosEdit((pos) => {
            RenderGun.translateAimPosition = new Vector3(pos.position.z, pos.position.y, pos.position.x) * 100F;
            RenderGun.rotateAimPosition = pos.rotation;
        });
    }
    
    public void EditSprintPos()
    {
        UIConfigManger.instance.StartPosEdit((pos) => {
            RenderGun.sprintTranslate = new Vector3(pos.position.z, pos.position.y, pos.position.x) * 100F;
            RenderGun.sprintRotate = pos.rotation;
        });
    }
    
}
