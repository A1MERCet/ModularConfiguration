using UnityEngine;

public class BehaviourMWFAtt: BehaviourMWF
{
    public MWFTypeAtt ConfigAtt => Config as MWFTypeAtt;
    private RenderTexture _scopeRT;
    private Camera _scopeCamera;
    
    protected override void Start()
    {
        base.Start();
        if (HasNode("overlayModel"))
        {
            _scopeRT = Instantiate(ResourceManager.instance.ui.RESOURCE_RT_SCOPE);
            var scopeModel = GetNode("scopeModel");
            var overlayModel = GetNode("overlayModel");
            _scopeCamera = new GameObject("ScopeCamera").AddComponent<Camera>();
            _scopeCamera.transform.SetParent(scopeModel, false);
            _scopeCamera.targetTexture = _scopeRT;

            var cameraMesh = scopeModel.GetComponent<MeshRenderer>();
            cameraMesh.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            cameraMesh.material.mainTexture = _scopeRT;

            var overlayMesh = overlayModel.GetComponent<MeshRenderer>();
            overlayMesh.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            UIMWFResource.instance.LoadTexture(
                "C:/workspace/ProjectBR/Source/Code/ModularConfiguration/Assets/Project/Local/MWFPackage/TestPack/assets/modularwarfare/textures/overlay/tcp.pistol_scope.png",
                (tex) => {
                    overlayMesh.material.mainTexture = tex;
                });
            
            _scopeCamera.fieldOfView = 20F;
            _scopeCamera.transform.localPosition = new Vector3(10, 0, 0);
            _scopeCamera.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 180));
        }
        HideInChildren(new [] {
            // "overlayModel",
            "overlaySolidModel"
        });
    }

    private void OnDestroy()
    {
        _scopeRT.Release();
        Destroy(_scopeRT);
    }
}