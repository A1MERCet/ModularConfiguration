using UnityEngine;

public class UIGunAttSelect : MonoBehaviour
{
    public MWFTypeGun config;
    public MWFTypeGun Config => config;
    
    public RectTransform content;
    
    public void SetConfig(MWFTypeGun config)
    {
        this.config = config;
        UpdateAttachments();
    }

    public void UpdateAttachments()
    {
        
    }
}
