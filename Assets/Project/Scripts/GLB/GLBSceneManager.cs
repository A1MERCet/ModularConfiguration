using System;
using UnityEngine;

public class GLBSceneManager : SingletonMono<GLBSceneManager>
{
    public Transform runtimeGLBTransform;

    private void Start()
    {
        runtimeGLBTransform = new GameObject("RuntimeGLB").transform;
    }

    public void Load(string path, string fileName, Action<GLBScene> onLoaded = null) {
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
