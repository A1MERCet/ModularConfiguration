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
            case "guns": {
                foreach (var type in mwfPackage.Configs.Values)
                    if (type is MWFTypeGun typeGun)
                        CreateUIMWFTypeRender(typeGun);
                break;
            }
            case "attachments": {
                foreach (var type in mwfPackage.Configs.Values)
                    if (type is MWFTypeAtt typeAtt)
                        CreateUIMWFTypeRender(typeAtt);
                break;
            }
        }
    }

    private void CreateUIMWFTypeRender(MWFTypeRender config)
    {
        var clone = Instantiate(ResourceManager.instance.ui.RESOURCE_TYPE_RENDER.gameObject, content);
        var item = clone.GetComponent<UIMWFTypeRender>();
        item.SetType(config);
        item.onClick += () => ModularConfiguration.instance.SetEditConfig(config);
        LoadIcon(item.icon, Path.Combine(config.package.iconPath, config.GetConfigType(), $"{config.InternalName}.png"));
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
        raw.color = Color.clear;
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
        raw.color = Color.white;
    }

    public void ActionSwitchType(string type)
    {
        showType = type;
        UpdateContent();
    }
    
}
