using System;
using UnityEngine;
using UnityGLTF;
using UnityGLTF.Loader;

[RequireComponent(typeof(GLTFComponent))]
public class GLBLoader: SingletonMono<GLBLoader>
{
    public GameObject loadedGLTF;
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
    }
    
    public async void Load(string path, string fileName, Action<GameObject> onLoaded = null)
    {
        Debug.Log($"加载GLB {fileName}");
        _gltf.GLTFUri = path;
        var opts = new ImportOptions();
        opts.AnimationMethod = AnimationMethod.Legacy;
        opts.DataLoader = new UnityWebRequestLoader(path);
        var import = new GLTFSceneImporter(fileName, opts);
        await import.LoadSceneAsync();
            
        Debug.Log($"加载 {fileName} 完成");
        OnLoaded(import.LastLoadedScene);
        onLoaded?.Invoke(import.LastLoadedScene);
        try {
       
        }catch (Exception e) {
            Debug.LogException(e);
        }
    }
}