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
                foreach (var type in mwfPackage.Configs.Values)
                    if (type is MWFTypeGun typeGun)
                    {
                        var clone = Instantiate(ResourceManager.instance.ui.RESOURCE_TYPE_GUN.gameObject, content);
                        var item = clone.GetComponent<UIMWFTypeGun>();
                        item.SetType(typeGun);
                        item.onClick += () => ModularConfiguration.instance.SetEditConfig(typeGun);
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
