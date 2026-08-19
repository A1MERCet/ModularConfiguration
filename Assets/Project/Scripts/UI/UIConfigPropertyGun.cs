
using Project;
using UnityEngine;

public class UIConfigPropertyGun : UIConfigProperty
{
    public MWFTypeGun ConfigGun => type as MWFTypeGun;
    public MWFRenderGun RenderGun => ConfigGun?.RenderGun;

    public void EditHipPos()
    {
        UIConfigManger.instance.EditBehaviour.updatePos = false;
        var defPos = new IncrementPos.Pos(UtilMC.Location2Vector(RenderGun.translateHipPosition), UtilMC.Rotation2Vector(RenderGun.rotateHipPosition));
        var editProperty = new UIConfigManger.EditProperty("根姿态") {
            defPos = defPos,
            onEditApply = (pos) => {
                RenderGun.translateHipPosition = new Vector3(pos.position.z, pos.position.y, pos.position.x) * 100F;
                RenderGun.rotateHipPosition = pos.rotation;
                UIConfigManger.instance.EditBehaviour.updatePos = true;
            }, 
            onEditCancel = (pos) => {
                UIConfigManger.instance.EditBehaviour.incrementPosModify["showcase"].position = Vector3.zero;
                UIConfigManger.instance.EditBehaviour.incrementPosModify["showcase"].rotation = Vector3.zero;
                UIConfigManger.instance.EditBehaviour.updatePos = true;
            }
        };
        UIConfigManger.instance.StartPosEdit(editProperty);
    }
    
    public void EditAimPos()
    {
        var defPos = new IncrementPos.Pos();
        defPos.position = UtilMC.Location2Vector(RenderGun.translateAimPosition);
        defPos.rotation = UtilMC.Rotation2Vector(RenderGun.rotateAimPosition);
        var editProperty = new UIConfigManger.EditProperty("瞄准姿态") {
            defPos = defPos,
            onEditApply = (pos) => {
                RenderGun.translateAimPosition = new Vector3(pos.position.z, pos.position.y, pos.position.x) * 100F;
                RenderGun.rotateAimPosition = pos.rotation;
            }
        };
        UIConfigManger.instance.StartPosEdit(editProperty);
    }
    
    public void EditSprintPos()
    {
        var defPos = new IncrementPos.Pos();
        defPos.position = UtilMC.Location2Vector(RenderGun.sprintTranslate);
        defPos.rotation = UtilMC.Rotation2Vector(RenderGun.sprintRotate);
        var editProperty = new UIConfigManger.EditProperty("奔跑姿态") {
            defPos = defPos,
            onEditApply = (pos) => {
                RenderGun.sprintTranslate = new Vector3(pos.position.z, pos.position.y, pos.position.x) * 100F;
                RenderGun.sprintRotate = pos.rotation;
            }
        };
        UIConfigManger.instance.StartPosEdit(editProperty);
    }
    
}
