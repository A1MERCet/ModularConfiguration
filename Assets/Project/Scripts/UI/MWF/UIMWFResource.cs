using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

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
                        LoadIcon(item.icon, Path.Combine(type.package.iconPath, "guns", $"{type.InternalName}.png"));
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
    
    public async void LoadIcon(RawImage raw, string path)
    {
        
        if (!File.Exists(path)) {
            raw.texture = null;
            raw.color = Color.clear;
            return;
        }
        byte[] fileData = await File.ReadAllBytesAsync(path);
        await Task.Yield(); 
        var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        if (texture.LoadImage(fileData)) {
            raw.texture = texture;
            raw.color = Color.white;
        }else {
            raw.texture = null;
            raw.color = Color.clear;
            Destroy(texture);
        }
    }
    
}
