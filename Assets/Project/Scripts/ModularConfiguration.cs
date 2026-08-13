public class ModularConfiguration : SingletonMono<ModularConfiguration>
{
    public GLBAnimationPlayer glbPlayer;
    public UIGLBPlayer uiGLBPlayer;

    public MWFProperty mwfProperty = new MWFProperty();
    
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

            var package = MWFPackageManager.instance.LoadPackage("D:\\Unity\\Project\\ModularConfiguration\\Assets\\Project\\Local\\MWFPackage\\TestPack");
            UIMWFResource.instance.SetMWFPackage(package);
        }
    }
}
