using System.IO;
using Dummiesman;
using Unity.VisualScripting;
using UnityEngine;

public class ModularConfiguration : SingletonMono<ModularConfiguration>
{
    public GLBAnimationPlayer glbPlayer;
    public UIGLBPlayer uiGLBPlayer;

    public MWFProperty mwfProperty = new MWFProperty();

    public MWFType editType;
    public BehaviourMWF editBehaviourMWF;
    
    void Start()
    {
        uiGLBPlayer.SetPlayer(glbPlayer);
    }

    private bool _init = false;
    void Update()
    {
        if (!_init)
        {
            _init = true;
            
            // var package = MWFPackageManager.instance.LoadPackage("D:\\Unity\\Project\\ModularConfiguration\\Assets\\Project\\Local\\MWFPackage\\TestPack");
            var package = MWFPackageManager.instance.LoadPackage("C:\\workspace\\ProjectBR\\Source\\Code\\ModularConfiguration\\Assets\\Project\\Local\\MWFPackage\\TestPack");
            UIMWFResource.instance.SetMWFPackage(package);
        }
    }

    public void SetEditConfig(MWFTypeGun typeGun)
    {
        GLBSceneManager.instance.ClearGLBScenes();
        TexturePBR texture = new TexturePBR() {
            baseColorPath = Path.Combine(typeGun.package.skinPath, "guns", $"{typeGun.InternalName}.png")
        };
        typeGun.RenderGun.LoadGLB((scene) => {
            var behaviour = scene.GetComponent<BehaviourMWFGun>();
            glbPlayer.SetBehaviourMWFGun(behaviour);
            UIGunConfiguration.instance.SetConfig(typeGun);
        });
    }
}
