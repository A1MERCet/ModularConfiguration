using System;
using Project;
using UnityEngine;
using UnityEngine.UI;

public class UIConfigManger : SingletonMono<UIConfigManger>
{
    public class EditProperty
    {
        public string name;

        public EditProperty(string name)
        {
            this.name = name;
        }

        public IncrementPos.Pos defPos;
        public Action<IncrementPos.Pos> onEditApply;
        public Action<IncrementPos.Pos> onEditUpdate;
        public Action<IncrementPos.Pos> onEditCancel;
    }
    
    public RectTransform content;
    public UIConfigProperty uiConfigProperty;

    public Vector3 prevMousePos;
    public Vector3 mouseVelocity;
    public IncrementPos.Pos editPos = new();
    private float _showcaseFOV = 0F;
    private EditProperty _editProperty;

    public RectTransform rectPanelLeft;
    public RectTransform rectPanelRight;
    public RectTransform rectGameScreen;
    private bool _hidePanel = false;
    
    public BehaviourMWF EditBehaviour => ModularConfiguration.instance.editBehaviourMWF;
    public UIMouseDetector mouseDetector;

    public RectTransform rectPosModificationTip;
    public Text textEditName;
    public Text textPosModificationPosition;
    public Text textPosModificationRotation;
    
    public RectTransform rectFOV;
    private RectTransform rectFOVPaent;
    public Text textFOV;

    private void Start()
    {
        rectFOVPaent = rectFOV.parent.GetComponent<RectTransform>();
    }

    private void Update()
    {
        mouseVelocity = Input.mousePosition - prevMousePos;
        if (EditBehaviour != null) {
            if (mouseDetector.Hoverd) {
                float multi = Input.GetKey(KeyCode.LeftShift) ? 0.1F : Input.GetKey(KeyCode.LeftControl) ? 5F : 1F;
                if (Input.GetMouseButton(1) && mouseVelocity.magnitude != 0) {
                    if (!Editing) StartPosEdit("自由检视");
                    editPos.rotation += new Vector3(mouseVelocity.y,-mouseVelocity.x ,0) * 0.25F * multi;
                } else if (Input.GetMouseButton(0) && mouseVelocity.magnitude != 0) {
                    if (!Editing) StartPosEdit("自由检视");
                    editPos.position += new Vector3(mouseVelocity.x,mouseVelocity.y ,0) * 0.0001F * multi;
                } else if (Input.mouseScrollDelta.y != 0F) {
                    _showcaseFOV += -Input.mouseScrollDelta.y * multi;
                } else if (Input.GetMouseButtonDown(2)) {
                    editPos.position = Vector3.zero;
                    editPos.rotation = Vector3.zero;
                    _showcaseFOV = 0F;
                }
            }
         
            EditBehaviour.incrementPosModify["showcase"].position = Vector3.Lerp(EditBehaviour.incrementPosModify["showcase"].position, editPos.position, Time.deltaTime * 20F);
            EditBehaviour.incrementPosModify["showcase"].rotation = Vector3.Lerp(EditBehaviour.incrementPosModify["showcase"].rotation, editPos.rotation, Time.deltaTime * 20F);
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            Set2AimPosition();
        } else if (Input.GetKeyDown(KeyCode.T)) {
            ActionHidePanel();
        }

        FPSCameraController.instance.incrementFOV.Set("showcase", Mathf.Lerp(FPSCameraController.instance.incrementFOV["showcase"],  _showcaseFOV, Time.deltaTime * 10F));
        prevMousePos = Input.mousePosition;

        rectFOV.sizeDelta = new Vector2(0, Mathf.Clamp(FPSCameraController.instance.camera.fieldOfView / 100F, 0F, 1F) * rectFOVPaent.rect.size.y);
        textFOV.text = $"{FPSCameraController.instance.camera.fieldOfView:0}<size=16>FOV</size>";

        if (Editing) {
            rectPosModificationTip.gameObject.SetActive(true);
            textPosModificationPosition.text = $" 位置 {editPos.position.x * 100F:0.00} {editPos.position.y * 100F:0.00} {editPos.position.z * 100F:0.00}";
            textPosModificationRotation.text = $" 旋转 {editPos.rotation.z * 100F:0.00} {editPos.rotation.y * 100F:0.00} {editPos.rotation.z * 100F:0.00}";
            textEditName.text = _editProperty.name;
        }else {
            textPosModificationPosition.text = "";
            textPosModificationRotation.text = "";
            rectPosModificationTip.gameObject.SetActive(false);
            textEditName.text = "-";
        }
        
        rectPanelLeft.anchoredPosition = Vector3.Lerp(rectPanelLeft.anchoredPosition,  new Vector3(_hidePanel ? -500 : 0, rectPanelLeft.anchoredPosition.y, 0), Time.deltaTime * 20F);
        rectPanelRight.anchoredPosition = Vector3.Lerp(rectPanelRight.anchoredPosition, new Vector3(_hidePanel ? 500 : 0, rectPanelRight.anchoredPosition.y, 0), Time.deltaTime * 20F);
        rectGameScreen.sizeDelta = Vector3.Lerp(rectGameScreen.sizeDelta, new Vector3(_hidePanel ? 0 : -(rectPanelLeft.sizeDelta.x + rectPanelRight.sizeDelta.x), rectGameScreen.sizeDelta.y, 0), Time.deltaTime * 20F);
    }

    public void SetConfig(MWFType config)
    {
        ClearConfig();
        ResourceManager.instance.ui.RESOURCE_CONFIGURATION.TryGetValue(config.GetConfigType(), out UIConfigProperty ui);
        if (ui != null)
        {
            var clone = Instantiate(ui.gameObject, content).GetComponent<UIConfigProperty>();
            clone.SetConfig(config);
            uiConfigProperty = clone;
        }

        if (config is MWFTypeGun) {
            WorldController.instance.shootingRange.gameObject.SetActive(true);
            UIAttachModify.instance.SetConfig(EditBehaviour as BehaviourMWFGun);
        }else {
            WorldController.instance.shootingRange.gameObject.SetActive(false);
        }
        
    }

    public void ClearConfig()
    {
        CancelEditPos();
        UIAttachModify.instance.SetConfig(null);
        if (uiConfigProperty) Destroy(uiConfigProperty.gameObject);
    }

    public void Set2AimPosition()
    {
        if (ModularConfiguration.instance.editConfig is MWFTypeGun configGun) {
            configGun.RenderGun.translateAimPosition = new Vector3(editPos.position.z, editPos.position.y, editPos.position.x) * 100F;
            configGun.RenderGun.rotateAimPosition = editPos.rotation;
        }
    }

    public void StartPosEdit(string name) => StartPosEdit(new EditProperty(name));
    public void StartPosEdit(EditProperty property)
    {
        CancelEditPos();
        if (property.defPos != null)
        {
            this.editPos.position = property.defPos.position;
            this.editPos.rotation = property.defPos.rotation;
        }
        this._editProperty = property;
    }

    public void ApplyEditPos()
    {
        if (Editing) {
            _editProperty.onEditApply?.Invoke(editPos);
            _editProperty = null;
            ResetEditPos();
        }
    }
    
    public void CancelEditPos()
    {
        if (Editing) {
            _editProperty.onEditCancel?.Invoke(editPos);
            _editProperty = null;
            ResetEditPos();
        }
    }
    
    public void ResetEditPos()
    {
        ResetEditPosition();
        ResetEditRotation();
    }
    public void ResetEditPosition() => editPos.position = Vector3.zero;
    public void ResetEditRotation() => editPos.rotation = Vector3.zero;
    public bool Editing => _editProperty != null;

    public void ActionHidePanel() => _hidePanel = !_hidePanel;
}
