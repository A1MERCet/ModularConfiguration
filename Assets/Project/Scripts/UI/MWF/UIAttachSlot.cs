using UnityEngine;
using UnityEngine.UI;

public class UIAttachSlot : MonoBehaviour
{
    public string slotID;
    public MWFTypeAtt installed;
    public RectTransform rect;

    public Text textSlotID;
    public Text textInstalledAttach;

    public RawImage imageIcon;
    public Image imageLine;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        textSlotID.text = slotID;
        textInstalledAttach.text = installed?.DisplayName ?? "";
        imageIcon.color = installed == null ? Color.clear : Color.white;
    }
}
