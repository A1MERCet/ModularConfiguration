using UnityEngine;

public class ModularConfiguration : SingletonMono<ModularConfiguration>
{
    public GLBAnimationPlayer glbPlayer;
    public UIGLBPlayer uiGLBPlayer;

    public MWFProperty mwfProperty = new MWFProperty();

    public Transform editScene;
    public MWFType editConfig;
    public BehaviourMWF editBehaviourMWF;
    
    private bool _init = false;
    
    void Start()
    {
        uiGLBPlayer.SetPlayer(glbPlayer);
    }
    
    void Update()
    {
        if (!_init)
        {
            _init = true;
            
            // var package = MWFPackageManager.instance.LoadPackage("D:\\Unity\\Project\\ModularConfiguration\\Assets\\Project\\Local\\MWFPackage\\TestPack");
            var package = MWFPackageManager.instance.LoadPackage("D:\\Unity\\Project\\ModularConfiguration\\Assets\\Project\\Local\\MWFPackage\\OfficialPack");
            // var package = MWFPackageManager.instance.LoadPackage("C:\\workspace\\ProjectBR\\Source\\Code\\ModularConfiguration\\Assets\\Project\\Local\\MWFPackage\\TestPack");
            UIMWFResource.instance.SetMWFPackage(package);
        }
    }

    public void SetEditConfig(MWFType config)
    {
        editConfig = config;
        ClearEitScene();
        if (config is MWFTypeRender tr)
        {
            if (tr.configRender is MWFRenderGLB rglb) {
                rglb.LoadGLB((scene) => {
                    var behaviour = scene.GetComponent<BehaviourGLB>();
                    editBehaviourMWF = behaviour;
                    glbPlayer.SetBehaviourGLB(behaviour);
                    UIConfigManger.instance.SetConfig(tr);
                    scene.transform.parent = editScene;
                    scene.transform.localPosition = new Vector3(0, 0, 0);
                    scene.transform.localRotation = Quaternion.identity;
                });
            } else if (tr.configRender is MWFRenderOBJ robj) {
                var loadedObj = robj.LoadOBJ();
                editBehaviourMWF = loadedObj;
                UIConfigManger.instance.SetConfig(tr);
                loadedObj.transform.parent = editScene;
                loadedObj.incrementPos.defaultValue.position = new Vector3(0, 0.01F, 0.1F);
                loadedObj.transform.localRotation = Quaternion.identity;
                uiGLBPlayer.Timeline.ClearMarks();
            }
        }
    }

    public void ClearEitScene()
    {
        foreach (Transform t in editScene)
            Destroy(t.gameObject);
    }
}
