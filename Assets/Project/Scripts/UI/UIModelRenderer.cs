using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIModelRenderer : MonoBehaviour
{
    public RawImage image;
    public GameObject model;
    public RectTransform loading;
    public ModelRenderer.RenderParameters renderParameters;
    [NonSerialized] ModelRenderer.RenderState renderState;

    private void Awake()
    {
        image.enabled = false;
        loading?.gameObject.SetActive(true);
    }
    
    protected virtual void Start()
    {
        if (model) Render();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && model && !model.IsDestroyed()) Render();
    }

    public void Render()
    {
        renderState = ModelRenderer.instance.Render(model, renderParameters);
        renderState.onRenderComplete += () => {
            image.enabled = true;
            loading?.gameObject.SetActive(false);
            model?.gameObject.SetActive(false);
            image.texture = renderState.texture;
        };
    }
    
}