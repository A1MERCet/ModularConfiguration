using System.Collections.Generic;
using Project;
using UnityEngine;

public class UIGunConfiguration : SingletonMono<UIGunConfiguration>
{
    public LoggerProxy logger = new LoggerProxy("MWF配置界面");
    public MWFTypeGun type;
    public MWFRenderGun Render => type?.renderGun;

    
    [Header("不需要修改 runtime自动创建")]
    public SerializableDic<string, GUIInput> properties = new();
    
    void Start()
    {
        foreach (GUIInput input in GetComponentsInChildren<GUIInput>())
        {
            if (!properties.TryAdd(input.propertyPath, input)) { logger.Error($"重复的属性名: {input.propertyPath}"); continue; }
            input.onValueChanged.AddListener(OnInputValueChanged);
        }
    }

    public void SetConfig(MWFTypeGun type)
    {
        ClearEventHandler();
        this.type = type;
        InitEventHandler();
    }
    
    public void OnRenderPropertyValueChanged(string path, object value) => properties[path]?.SetValue(value);
    public void OnTypePropertyValueChanged(string path, object value) => properties[path]?.SetValue(value);
    
    void Update()
    {
      
    }

    private void OnDestroy()
    {
        ClearEventHandler();
        foreach (var input in properties.Values)
            input.onValueChanged.RemoveListener(OnInputValueChanged);
    }

    private void ClearEventHandler()
    {
        if (Render != null) Render.onPropertyChanged -= OnRenderPropertyValueChanged;
        if (type != null) type.onPropertyChanged -= OnTypePropertyValueChanged;
    }
    private void InitEventHandler()
    {
        foreach (GUIInput input in GetComponentsInChildren<GUIInput>())
        {
            MWFConfig cfg = null;
            switch (input.configType)
            {
                case MWFConfigType.TYPE: cfg = type; break;
                case MWFConfigType.RENDER: cfg = Render; break;
            }
            if (cfg != null) input.SetValue(cfg.JsonObject?.Get(input.propertyPath)); 
        }

        if (Render != null) Render.onPropertyChanged += OnRenderPropertyValueChanged;
        if (type != null) type.onPropertyChanged += OnTypePropertyValueChanged;
    }
    private void OnInputValueChanged(GUIInput input, object value)
    {
        logger.Info($"输入更新 {input.propertyPath}: {value ?? "null"}({value?.GetType().Name ?? "null"})");
        switch (input.configType)
        {
            case MWFConfigType.TYPE:
            {
                type.JsonObject.Set(input.propertyPath, value);
                type.OnPropertyChanged(input.propertyPath, value);
                break;
            } case MWFConfigType.RENDER: {
                Render.JsonObject.Set(input.propertyPath, value);
                Render.OnPropertyChanged(input.propertyPath, value);
                break;
            }
        }
    }

}
