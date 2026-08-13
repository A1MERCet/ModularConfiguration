using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIGLBPlayerTimeline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public struct Mark
    {
        public string id;
        public string name;
        public int start;
        public int end;
        public Color color;
        public override string ToString() => JsonUtility.ToJson(this);
    }
    
    public UIGLBPlayer parent;
    private readonly List<Mark> marks = new List<Mark>();
    public List<Mark> Marks => marks;
    public RectTransform rect;
    public RectTransform rectMarkContent;

    public float process;
    public float maxFrame = 0F;
    public float showFrame = 0F;

    private bool _input = false;

    public void SetMaxFrame(float v)
    {
        maxFrame = v;
        showFrame = 200F;
    }
    
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }
    
    void Update()
    {
        if (Input.mouseScrollDelta.y != 0) {
            showFrame = Mathf.Clamp(showFrame + (showFrame * -Input.mouseScrollDelta.y * 0.1F), 100F, maxFrame * 2F);
            UpdateMarkInstances();
        }
    }
    
    private void UpdateMarkInstances()
    {
        ClearMarkInstances();
        marks.ForEach(e=>CreateMark(e));
    }

    private void ClearMarkInstances()
    {
        foreach (Transform t in rectMarkContent.transform)
            Destroy(t.gameObject);
    }
    
    public void RemoveMark(string id)
    {
        marks.RemoveAll(m => m.id == id);
        RemoveMarkInstance(id);
    }
    
    public void RemoveMarkInstance(string id)
    {
        var find = rectMarkContent.Find(id);
        if (find != null) Destroy(find.gameObject);
    }
    
    public bool AddMark(Mark m)
    {
        foreach (var mark in marks)
            if (mark.id == m.id)
                return false;
        marks.Add(m);
        CreateMark(m);
        return true;
    }

    public void SetMarks(List<Mark> marks)
    {
        ClearMarkInstances();
        this.marks.Clear();
        marks.ForEach(e => AddMark(e));
    }

    public UIAnimaStageMark CreateMark(Mark m)
    {
        float width = rectMarkContent.rect.size.x * (showFrame <= 0 ? 0 : Mathf.Clamp(Math.Abs(m.end - m.start) / showFrame, 0F, 1F));
        float x = rectMarkContent.rect.size.x * (showFrame <= 0 ? 0 : (m.start / showFrame));
        var animaStageMark = Instantiate(ResourceManager.instance.ui.RESOURCE_ANIMA_STAGE_MARK.gameObject, rectMarkContent.transform).GetComponent<UIAnimaStageMark>();
        var animaStageRect = animaStageMark.GetComponent<RectTransform>();
        animaStageRect.sizeDelta = new Vector2(width, animaStageRect.sizeDelta.y);
        animaStageRect.localPosition = new Vector3(x - rectMarkContent.rect.size.x / 2F, 0F, 0F);
        animaStageMark.name = m.id;
        animaStageMark.text.text = m.name;
        animaStageMark.color = ModularConfiguration.instance.mwfProperty.GetAnimaStageColor(m.id);
        animaStageMark.SetColorDefault();
        return animaStageMark;
    }

    public void OnPointerEnter(PointerEventData eventData) => _input = true;
    public void OnPointerExit(PointerEventData eventData) => _input = false;
}
