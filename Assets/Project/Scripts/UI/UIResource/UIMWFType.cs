using System;
using UnityEngine;
using UnityEngine.UI;

public class UIMWFType<T> : MonoBehaviour where T : MWFType
{
    private T mwfType;

    public Text textID;
    public Text textName;
    public Button button;
    public Action onClick;

    public virtual void SetType(T type)
    {
        this.mwfType = type;
    }
    
    protected virtual void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => onClick?.Invoke());
    }

    protected virtual void Update()
    {
        textID.text = mwfType?.InternalName ?? "";
        textName.text = mwfType?.DisplayName ?? "";
    }
}
