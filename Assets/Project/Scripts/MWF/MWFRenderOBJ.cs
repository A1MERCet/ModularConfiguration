using System.IO;
using Dummiesman;
using UnityEngine;

public abstract class MWFRenderOBJ: MWFRender
{
    public BehaviourMWF loadedOBJ;
    public override BehaviourMWF LoadedModel() => loadedOBJ;

    public virtual BehaviourMWF LoadOBJ()
    {
        var path = Path.Combine(configType.package.objPath, GetConfigType(), $"{ModelFileName}");
        Debug.Log($"准备加载OBJScene {path} {configType.InternalName}.obj");

        var obj = new OBJLoader().Load(path);
        obj.transform.localScale = Vector3.one * 0.01F;
        TexturePBR texture = new TexturePBR() {
            baseColorPath = Path.Combine(configType.package.skinPath,  GetConfigType(), configType.modelSkins.Length > 0 ? $"{configType.modelSkins[0].skinAsset}.png" : "")
        };
        Debug.Log($"加载纹理 {texture.baseColorPath}");
        texture.LoadAsync();
        texture.onLoaded += () => {
            foreach (var mesh in obj.GetComponentsInChildren<MeshRenderer>())
            {
                string originName = mesh.material?.name ?? null;
                var material = new Material(Shader.Find("Universal Render Pipeline/Autodesk Interactive/AutodeskInteractive"));
                material.name = originName ?? mesh.name;
                material.EnableKeyword("_USECOLORMAP_ON");
                material.SetFloat("_UseColorMap", 1F);
                material.SetFloat("_UseMetallicMap", 1F);
                material.SetFloat("_UseRoughnessMap", 1F);
                material.SetColor("_Color", Color.white);
                material.SetTexture("_MainTex", texture.baseColor);
                mesh.material = material;
            }
        };
        loadedOBJ = PostLoadOBJ(obj);
        loadedOBJ.model = obj;
        return loadedOBJ;
    }
    protected virtual BehaviourMWF PostLoadOBJ(GameObject o) => o.AddComponent<BehaviourMWF>();
}