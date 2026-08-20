using System;
using System.Collections.Generic;
using UnityEngine;

public class GLBSceneManager : SingletonMono<GLBSceneManager>
{
    public Transform runtimeGLBTransform;

    private void Start()
    {
        runtimeGLBTransform = new GameObject("RuntimeGLB").transform;
    }

    public void Load(string path, string fileName, TexturePBR texture, HashSet<string> skipMaterials, Action<GLBScene> onLoaded = null) {
        Debug.Log($"准备加载GLBScene {path} {fileName}");
        GLBLoader.instance.Load(path, fileName, (obj) =>
        {
            var glbSceneObj = new GameObject(fileName);
            glbSceneObj.transform.SetParent(runtimeGLBTransform, false);
            obj.transform.SetParent(glbSceneObj.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one * 0.01F;
            var glbScene = glbSceneObj.AddComponent<GLBScene>();
            glbScene.SetGLTF(obj.transform);
            
            // Debug.Log($"加载纹理 {fileName}");
            texture.onLoaded = () => {
                foreach (var mesh in obj.GetComponentsInChildren<MeshRenderer>())
                {
                    if (skipMaterials.Contains(mesh.name)) continue;
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
                foreach (var mesh in obj.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    if (skipMaterials.Contains(mesh.name)) continue;
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
                    mesh.rootBone = obj.transform;
                    var bounds = mesh.bounds;
                    bounds.center = Vector3.zero;
                    bounds.extents = Vector3.one * 1000F;
                    mesh.bounds = bounds;
                }
            };
            texture.LoadAsync();
            
            Debug.Log($"加载GLBScene完成 {fileName}");
            onLoaded?.Invoke(glbScene);
        });
    }

    public void ClearGLBScenes()
    {
        foreach (Transform t in runtimeGLBTransform)
            Destroy(t.gameObject);
    }
}
