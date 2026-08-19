using System.Collections.Generic;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIAttachModify : SingletonMono<UIAttachModify>
{
    public RectTransform content;
    public List<UIAttachSlot> _slots = new();

    private MWFTypeGun _configGun;
    private BehaviourMWFGun _behaviour;
    private MWFRenderGun RenderGun => _configGun?.RenderGun;
    private Dictionary<string, List<MWFTypeAtt>> _allowAttachs = new();

    public void SetConfig(BehaviourMWFGun behaviour)
    {
        if (behaviour == null)
        {
            _configGun = null;
            _behaviour = null;
            _allowAttachs.Clear();
            ClearAttachs();
            return;
        }
        this._behaviour = behaviour;
        this._configGun = _behaviour?.ConfigGun;
        this._allowAttachs.Clear();

        var typeAttachs = _configGun.package.GetList<MWFTypeAtt>();
        foreach (var (k, v) in _configGun.acceptedAttachments)
        {
            List<MWFTypeAtt> allowAttachs = new();
            foreach (var attachID in v)
            {
                MWFTypeAtt find = typeAttachs.Find((e) => e.InternalName == attachID);
                if (find != null) allowAttachs.Add(find);
            }
            _allowAttachs.Add(k, allowAttachs);
        }
        
        ClearAttachs();
        CreateAttachs();
    }

    public void CreateAttachs()
    {
        if (_configGun == null) return;
        foreach (var slotID in _configGun.acceptedAttachments.Keys)
        {
            var slot = Instantiate(ResourceManager.instance.ui.RESOURCE_ATTACH_SLOT, content).GetComponent<UIAttachSlot>();
            slot.slotID = slotID;
            this._slots.Add(slot);
        }
    }

    public void ClearAttachs()
    {
        foreach (Transform t in content)
            Destroy(t.gameObject);
    }

    private void Update()
    {
        if (_behaviour == null || _configGun == null || RenderGun == null) {
            ClearAttachs();
            return;
        }
        if (Camera.main == null) return;

        var contentSize = content.rect.size;
        Vector2 screenCenter = new Vector2(Screen.width, Screen.height) * 0.5f;

        foreach (var slot in _slots)
        {
            if (slot.IsDestroyed()) continue;
            RenderGun.attachmentGroup.TryGetValue(slot.slotID, out MWFRenderGun.AttachSlot configAttach);
            if (configAttach == null) continue;

            Vector3 worldPosAttach = _behaviour.model.transform.position + UtilMC.Location2Vector(configAttach.Translate);
            Vector3 world2ScreenAttach = Camera.main.WorldToScreenPoint(worldPosAttach);
            Vector2 baseScreenPos = new Vector2(world2ScreenAttach.x, world2ScreenAttach.y);

            Vector2 dir = (baseScreenPos - screenCenter).normalized;
            Vector2 offsetScreenPos = baseScreenPos + dir * 0F;

            slot.rect.position = new Vector3(offsetScreenPos.x, offsetScreenPos.y, 0);
            slot.rect.anchoredPosition = new Vector2(
                Mathf.Clamp(slot.rect.anchoredPosition.x, -contentSize.x/2 + slot.rect.sizeDelta.x/2, contentSize.x/2 - slot.rect.sizeDelta.x/2),
                Mathf.Clamp(slot.rect.anchoredPosition.y, -contentSize.y/2 + slot.rect.sizeDelta.y/2, contentSize.y/2 - slot.rect.sizeDelta.y/2)
            );

            Debug.DrawLine(_behaviour.model.transform.position, worldPosAttach, Color.red);
            Debug.DrawLine(Camera.main.transform.position, worldPosAttach, Color.blue);
        }
    }
}
