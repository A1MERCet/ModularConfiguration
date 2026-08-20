using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
        foreach (var attachType in _configGun.acceptedAttachments.Keys)
        {
            UIAttachSlot slot = Instantiate(ResourceManager.instance.ui.RESOURCE_ATTACH_SLOT, content).GetComponent<UIAttachSlot>();
            slot.attachType = attachType;
            this._slots.Add(slot);
            
            _behaviour.attachments.TryGetValue(attachType, out BehaviourMWFAtt installed);
            slot.installed = installed?.ConfigAtt;
            slot.UpdateIcon();
            
            slot.onSelect += (s) => {
                var allowed = _allowAttachs[slot.attachType];
                var existing = _configGun.package.GetList<MWFTypeAtt>();

                List<UIGunAttSelect.AttachItem> list = new();
                foreach (var config in existing)
                    list.Add(new UIGunAttSelect.AttachItem() {
                        attach = config,
                        locked = !allowed.Contains(config)
                    });
                list.Sort((a, b) => {
                    int l = a.locked.CompareTo(b.locked);
                    if (l != 0) return l;
                    return string.CompareOrdinal(a.attach.InternalName, b.attach.InternalName);
                });

                UIGunAttSelect.instance.SetAttachmentList(list, (item) => {
                    _behaviour.AddAttachment(item.attach);
                    UIGunAttSelect.instance.Close();
                    
                    _behaviour.attachments.TryGetValue(attachType, out BehaviourMWFAtt installed);
                    slot.installed = installed?.ConfigAtt;
                    slot.UpdateIcon();
                });
            };
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
            RenderGun.attachmentGroup.TryGetValue(slot.attachType, out MWFRenderGun.AttachSlot configAttach);
            if (configAttach == null) continue;

            Transform bindPart = _behaviour.attachPositions[slot.attachType];
            Vector3 world2ScreenAttach = Camera.main.WorldToScreenPoint(bindPart.position);
            Vector2 screenPos = new Vector2(world2ScreenAttach.x, world2ScreenAttach.y);

            Vector2 dir = (screenPos - screenCenter).normalized;
            Vector2 offset = screenPos + dir * 300F;

            slot.rect.position = new Vector3(offset.x, offset.y, 0);
            slot.rect.anchoredPosition = new Vector2(
                Mathf.Clamp(slot.rect.anchoredPosition.x, -contentSize.x/2 + slot.rect.sizeDelta.x/2, contentSize.x/2 - slot.rect.sizeDelta.x/2),
                Mathf.Clamp(slot.rect.anchoredPosition.y, -contentSize.y/2 + slot.rect.sizeDelta.y/2, contentSize.y/2 - slot.rect.sizeDelta.y/2)
            );

            Debug.DrawLine(_behaviour.model.transform.position, bindPart.position, Color.red);
            Debug.DrawLine(Camera.main.transform.position, bindPart.position, Color.blue);
        }
        // foreach (var slot in _slots)
        // {
        //     if (slot.IsDestroyed()) continue;
        //     RenderGun.attachmentGroup.TryGetValue(slot.attachType, out MWFRenderGun.AttachSlot configAttach);
        //     if (configAttach == null) continue;
        //
        //     Transform bindPart = _behaviour.model.transform;
        //     Vector3 worldPosAttach = bindPart.TransformPoint(UtilMC.Location2Vector(configAttach.Translate * 100F));
        //     Vector3 world2ScreenAttach = Camera.main.WorldToScreenPoint(worldPosAttach);
        //     Vector2 screenPos = new Vector2(world2ScreenAttach.x, world2ScreenAttach.y);
        //
        //     Vector2 dir = (screenPos - screenCenter).normalized;
        //     Vector2 offset = screenPos + dir * 0F;
        //
        //     slot.rect.position = new Vector3(offset.x, offset.y, 0);
        //     slot.rect.anchoredPosition = new Vector2(
        //         Mathf.Clamp(slot.rect.anchoredPosition.x, -contentSize.x/2 + slot.rect.sizeDelta.x/2, contentSize.x/2 - slot.rect.sizeDelta.x/2),
        //         Mathf.Clamp(slot.rect.anchoredPosition.y, -contentSize.y/2 + slot.rect.sizeDelta.y/2, contentSize.y/2 - slot.rect.sizeDelta.y/2)
        //     );
        //
        //     Debug.DrawLine(bindPart.position, worldPosAttach, Color.red);
        //     Debug.DrawLine(Camera.main.transform.position, worldPosAttach, Color.blue);
        // }
    }
}
