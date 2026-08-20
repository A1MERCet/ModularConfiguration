using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UIGunAttSelect : SingletonMono<UIGunAttSelect>
{
    [Serializable]
    public struct AttachItem
    {
        public MWFTypeAtt attach;
        public bool locked;
    }

    public RectTransform root;
    public RectTransform content;
    private List<AttachItem> _attList = new();
    public Action<AttachItem> onSelect;

    private void Start()
    {
        root.gameObject.SetActive(false);
    }

    public void Close()
    {
        ClearContent();
    }
    
    public void ClearContent()
    {
        root.gameObject.SetActive(false);
        onSelect = null;
        _attList.Clear();
        foreach (Transform t in content)
            Destroy(t.gameObject);
    }
    
    public void SetAttachmentList(List<AttachItem> list, Action<AttachItem> onSelect = null)
    {
        ClearContent();
        this._attList = list;
        this.onSelect = onSelect;
        UpdateAttachments();
        root.gameObject.SetActive(true);
    }

    private void UpdateAttachments()
    {
        foreach (var item in _attList)
        {
            var ui = Instantiate(ResourceManager.instance.ui.RESOURCE_TYPE_ATT.gameObject, content).GetComponent<UIMWFTypeRender>();
            ui.SetType(item.attach);
            UIMWFResource.instance.LoadIcon(ui.icon, Path.Combine(item.attach.package.iconPath, item.attach.GetConfigType(), $"{item.attach.InternalName}.png"));
            ui.onClick += () => onSelect?.Invoke(item);
            if (item.locked)
                foreach (var btn in ui.GetComponentsInChildren<Button>())
                    btn.enabled = false;
        }
    }
}
