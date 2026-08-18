using System;
using System.Collections.Generic;
using UnityEngine;

public class GLBScene : MonoBehaviour
{
    private Transform _gltfRoot;
    private Animation _animation;
    
    private Dictionary<string, MeshRenderer> _meshRenderers = new();
    private Dictionary<string, SkinnedMeshRenderer> _skinnedMeshRenderers = new();

    public Transform GLTFRoot => _gltfRoot;
    public Animation Animation => _animation;
    public Dictionary<string, MeshRenderer> MeshRenderers => _meshRenderers;
    public Dictionary<string, SkinnedMeshRenderer> SkinnedMeshRenderers => _skinnedMeshRenderers;
    
    public void SetGLTF(Transform gltfRoot)
    {
        _gltfRoot = gltfRoot;
        UpdateIndex();
    }

    public void UpdateIndex()
    {
        _animation = null;
        _meshRenderers.Clear();
        _skinnedMeshRenderers.Clear();
        
        if (_gltfRoot) {
            _animation = _gltfRoot.GetComponent<Animation>();
            foreach (var mesh in _gltfRoot.GetComponentsInChildren<MeshRenderer>())
                _meshRenderers.Add(mesh.name, mesh);
            foreach (var mesh in _gltfRoot.GetComponentsInChildren<SkinnedMeshRenderer>())
                _skinnedMeshRenderers.Add(mesh.name, mesh);
        }
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
