using System;
using UnityEngine;
using UnityGLTF;
using UnityGLTF.Loader;

[RequireComponent(typeof(GLTFComponent))]
public class GLBLoader: SingletonMono<GLBLoader>
{
    public GameObject loadedGLTF;
    
    public TexutrePBR texturePBR;
    private GLTFComponent _gltf;

    private void Start()
    {
        _gltf = GetComponent<GLTFComponent>();
    }

    public void OnLoaded(GameObject obj)
    {
        this.loadedGLTF = obj;
        foreach (var c in UtilUnity.GetChildren(obj.transform))
            if (c.name == "flashModel") 
                c.gameObject.SetActive(false);

        texturePBR.onLoaded = () => {
            //todo
            foreach (var mesh in loadedGLTF.GetComponentsInChildren<MeshRenderer>())
            {
                string originName = mesh.material?.name ?? null;
                var material = new Material(Shader.Find("Universal Render Pipeline/Autodesk Interactive/AutodeskInteractive"));
                material.name = originName ?? mesh.name;
                material.EnableKeyword("_USECOLORMAP_ON");
                material.SetFloat("_UseColorMap", 1F);
                material.SetColor("_Color", Color.white);
                material.SetTexture("_MainTex", texturePBR.baseColor);
                mesh.material = material;
            }
            foreach (var mesh in loadedGLTF.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                string originName = mesh.material?.name ?? null;
                var material = new Material(Shader.Find("Universal Render Pipeline/Autodesk Interactive/AutodeskInteractive"));
                material.name = originName ?? mesh.name;
                material.EnableKeyword("_USECOLORMAP_ON");
                material.SetFloat("_UseColorMap", 1F);
                material.SetColor("_Color", Color.white);
                material.SetTexture("_MainTex", texturePBR.baseColor);
                mesh.material = material;
                mesh.rootBone = loadedGLTF.transform;
                var bounds = mesh.bounds;
                bounds.center = Vector3.zero;
                bounds.extents = Vector3.one * 1000F;
                mesh.bounds = bounds;
            }
        };
        texturePBR.LoadAsync();
        
    }
    
    public async void Load(string path, string fileName, Action<GameObject> onLoaded = null)
    {
        try {
            Debug.Log($"加载GLB {fileName}");
            _gltf.GLTFUri = path;
            var opts = new ImportOptions();
            opts.AnimationMethod = AnimationMethod.Legacy;
            opts.DataLoader = new UnityWebRequestLoader(path);
            var import = new GLTFSceneImporter(fileName, opts);
            await import.LoadSceneAsync();
            
            Debug.Log($"加载GLB {fileName} 完成");

            OnLoaded(import.LastLoadedScene);
            onLoaded?.Invoke(import.LastLoadedScene);
        }catch (Exception e) {
            Debug.LogError(e);
        }
    }
}