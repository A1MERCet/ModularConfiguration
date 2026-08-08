using System.IO;
using UnityEngine;

public class UIMWFResource : SingletonMono<UIMWFResource>
{
    private MWFPackage mwfPackage;
    public MWFPackage MWFPackage => mwfPackage;
    
    public RectTransform content;

    public string showType = "guns";

    public void SetMWFPackage(MWFPackage mwfPackage)
    {
        this.mwfPackage = mwfPackage;
        UpdateContent();
    }

    public void UpdateContent()
    {
        ClearContent();
        switch (showType)
        {
            case "guns":
            {
                foreach (var type in mwfPackage.Types)
                    if (type is MWFTypeGun mwfTypeGun)
                    {
                        var clone = Instantiate(ResourceManager.instance.ui.RESOURCE_TYPE_GUN.gameObject, content);
                        var item = clone.GetComponent<UIMWFTypeGun>();
                        item.SetType(mwfTypeGun);
                        item.onClick += () => {
                            MainThread.instance.Enqueue(() => {
                                var render = mwfPackage.GetMWFRender<MWFRenderGun>(mwfTypeGun.internalName);
                                GLBSceneManager.instance.ClearGLBScenes();
                                GLBSceneManager.instance.Load(Path.Combine(mwfPackage.glbPath, "guns"), render.modelFileName, (scene) => {
                                    ModularConfiguration.instance.glbPlayer.SetGLTF(scene);
                                });
                            });
                        };
                    }

                break;
            }
        }
    }

    public void ClearContent()
    {
        foreach (Transform t in content.transform)
            Destroy(t.gameObject);
    }
    
    
}
