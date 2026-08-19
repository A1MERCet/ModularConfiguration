using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UIGunAttSelect : SingletonMono<UIGunAttSelect>
{
    public struct AttListItem
    {
        public MWFTypeAtt att;
        public bool locked;
    }
    
    public RectTransform content;
    private List<AttListItem> _attList = new();
    public Action<AttListItem> onSelect;

    public void ClearContent()
    {
        onSelect = null;
        _attList.Clear();
        foreach (Transform t in content)
            Destroy(t.gameObject);
    }
    
    public void SetAttachmentList(List<AttListItem> list, Action<AttListItem> onSelect = null)
    {
        ClearContent();
        this._attList = list;
        this.onSelect = onSelect;
        UpdateAttachments();
    }

    private void UpdateAttachments()
    {
        foreach (var item in _attList)
        {
            var ui = Instantiate(ResourceManager.instance.ui.RESOURCE_TYPE_ATT.gameObject, content).GetComponent<UIMWFTypeRender>();
            ui.SetType(item.att);
            UIMWFResource.instance.LoadIcon(ui.icon, Path.Combine(item.att.package.iconPath, item.att.GetConfigType(), $"{item.att.InternalName}.png"));
            ui.onClick += () => onSelect?.Invoke(item);
        }
    }
}
