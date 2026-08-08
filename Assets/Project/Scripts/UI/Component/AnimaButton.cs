using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimaButton : MonoBehaviour ,IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public UIEffect effect;
    public Button button;
    
    private bool hoverd = false;
    private bool clicked = false;

    private Image image;

    public Color disableColor = Color.white;
    public Color disableOutlineColor = Color.white;
    
    public float hoverSmooth = 5F;
    public float clickSmooth = 10F;
    public float clickDuration = 0.2F;
    
    public Vector3 hoverScale = Vector3.one;
    public Vector3 clickScale = Vector3.one;

    private Color originalColor;
    public Color hoverColor = Color.white;
    public Color clickColor = Color.white;

    private Color originOutlineColor;
    public Color hoverOutlineColor = Color.white;

    private float originOutline;
    private float hoverOutline = 0.1F;
    
    private float _clickDuration = 0F;
    
    void Start()
    {
        image = GetComponent<Image>();
        effect = GetComponent<UIEffect>();
        originalColor = image?.color ?? Color.white;
        originOutlineColor = effect.edgeColor;
        originOutline = effect.edgeWidth;
    }

    void FixedUpdate()
    {
        if (!button.enabled) {
            image.color = Color.Lerp(image.color, disableColor, hoverSmooth * Time.fixedDeltaTime);
            
            effect.edgeColor = Color.Lerp(effect.edgeColor, disableOutlineColor, hoverSmooth * Time.fixedDeltaTime);
        } else if (_clickDuration > 0F) {
            _clickDuration -= Time.fixedDeltaTime;
            image.color = Color.Lerp(image.color, clickColor, clickSmooth * Time.fixedDeltaTime);
            image.transform.localScale = Vector3.Lerp(image.transform.localScale, clickScale, clickSmooth * Time.fixedDeltaTime);
        } else if (hoverd) {
            image.color = Color.Lerp(image.color, hoverColor, hoverSmooth * Time.fixedDeltaTime);
            image.transform.localScale = Vector3.Lerp(image.transform.localScale, hoverScale, hoverSmooth * Time.fixedDeltaTime);
            
            effect.edgeColor = Color.Lerp(effect.edgeColor, hoverOutlineColor, hoverSmooth * Time.fixedDeltaTime);
            effect.edgeWidth = Mathf.Lerp(effect.edgeWidth, hoverOutline, hoverSmooth * Time.fixedDeltaTime);
        }else {
            image.color = Color.Lerp(image.color, originalColor, hoverSmooth * Time.fixedDeltaTime);
            image.transform.localScale = Vector3.Lerp(image.transform.localScale, Vector3.one , hoverSmooth * Time.fixedDeltaTime);
            
            effect.edgeColor = Color.Lerp(effect.edgeColor, originOutlineColor, hoverSmooth * Time.fixedDeltaTime);
            effect.edgeWidth = Mathf.Lerp(effect.edgeWidth, originOutline, hoverSmooth * Time.fixedDeltaTime);
        }
    }
    
    // void Update()
    // {
    //     if (button.enabled) {
    //         image.color = Color.Lerp(image.color, disableColor, hoverSmooth * Time.deltaTime);
    //         
    //         effect.edgeColor = Color.Lerp(effect.edgeColor, disableOutlineColor, hoverSmooth * Time.deltaTime);
    //     } else if (_clickDuration > 0F) {
    //         _clickDuration -= Time.deltaTime;
    //         image.color = Color.Lerp(image.color, clickColor, clickSmooth * Time.deltaTime);
    //         image.transform.localScale = Vector3.Lerp(image.transform.localScale, clickScale, clickSmooth * Time.deltaTime);
    //     } else if (hoverd) {
    //         image.color = Color.Lerp(image.color, hoverColor, hoverSmooth * Time.deltaTime);
    //         image.transform.localScale = Vector3.Lerp(image.transform.localScale, hoverScale, hoverSmooth * Time.deltaTime);
    //         
    //         effect.edgeColor = Color.Lerp(effect.edgeColor, hoverOutlineColor, hoverSmooth * Time.deltaTime);
    //         effect.edgeWidth = Mathf.Lerp(effect.edgeWidth, hoverOutline, hoverSmooth * Time.deltaTime);
    //     }else {
    //         image.color = Color.Lerp(image.color, originalColor, hoverSmooth * Time.deltaTime);
    //         image.transform.localScale = Vector3.Lerp(image.transform.localScale, Vector3.one , hoverSmooth * Time.deltaTime);
    //         
    //         effect.edgeColor = Color.Lerp(effect.edgeColor, originOutlineColor, hoverSmooth * Time.deltaTime);
    //         effect.edgeWidth = Mathf.Lerp(effect.edgeWidth, originOutline, hoverSmooth * Time.deltaTime);
    //     }
    // }

    public void SetDisable() => button.enabled = false;
    public void SetEnable() => button.enabled = true;
    public void SetEnable(bool v) => button.enabled = v;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        clicked = true;
        hoverd = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverd = true;
        clicked = false;
        _clickDuration = 0F;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverd = false;
        clicked = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        clicked = true;
        _clickDuration = clickDuration;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        clicked = false;
    }
}
