using System;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIAttachSlot : MonoBehaviour
{
    public string attachType;
    public MWFTypeAtt installed;
    public RectTransform rect;

    public Text textSlotID;
    public Text textInstalledAttach;

    public RawImage imageIcon;
    public Image imageLine;

    public Action<UIAttachSlot> onSelect;
    
    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        textSlotID.text = attachType;
        textInstalledAttach.text = installed?.DisplayName ?? "";
    }

    public void UpdateIcon()
    {
        if (installed != null) UIMWFResource.instance.LoadIcon(imageIcon, Path.Combine(installed.package.iconPath, installed.GetConfigType(), $"{installed.InternalName}.png"));
        else imageIcon.color = Color.clear;
    }
    
    public void ActionSelect() => onSelect?.Invoke(this);
}
