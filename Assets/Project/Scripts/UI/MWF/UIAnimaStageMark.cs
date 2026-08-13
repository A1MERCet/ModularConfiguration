using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimaStageMark : MonoBehaviour
{
    public RectTransform rect;
    public UIEffect highlightEffect;
    public Image imageColorOverlay;

    public Color color = Color.white;
    public Color colorHighlight = Color.white;
    public Color colorInterval = Color.white;

    public Text text;
    
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        imageColorOverlay.color = new Color(color.r, color.g, color.b, 0.25F);
        highlightEffect.enabled = false;
    }

    public void SetColorDefault()
    {
        Color c = new Color(color.r, color.g, color.b, 0.25F);
        imageColorOverlay.color = c;
        highlightEffect.enabled = false;
    }
    
    public void SetColorHighlight()
    {
        imageColorOverlay.color = colorHighlight;
        highlightEffect.enabled = true;
    }
    
    public void SetColorInterval()
    {
        imageColorOverlay.color = colorInterval;
        highlightEffect.enabled = false;
    }
}
