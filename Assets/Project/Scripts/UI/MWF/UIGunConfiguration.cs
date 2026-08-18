using System.Collections.Generic;
using Project;
using UnityEngine;

public class UIGunConfiguration : SingletonMono<UIGunConfiguration>
{
    public LoggerProxy logger = new LoggerProxy("MWF配置界面");
    public MWFTypeGun type;
    public MWFRenderGun ConfigRender => type?.configRender as MWFRenderGun;

    
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

    protected override void OnDestroy()
    {
        base.OnDestroy();
        ClearEventHandler();
        foreach (var input in properties.Values)
            input.onValueChanged.RemoveListener(OnInputValueChanged);
    }

    private void ClearEventHandler()
    {
        if (ConfigRender != null) ConfigRender.onPropertyChanged -= OnRenderPropertyValueChanged;
        if (type != null) type.onPropertyChanged -= OnTypePropertyValueChanged;
    }
    private void InitEventHandler()
    {
        foreach (GUIInput input in GetComponentsInChildren<GUIInput>())
        {
            MWFConfig cfg = null;
            if (input.isType) cfg = type;
            else if (input.isRender) cfg = ConfigRender;
            if (cfg != null) input.SetValue(cfg.JsonObject?.Get(input.propertyPath)); 
        }

        if (ConfigRender != null) ConfigRender.onPropertyChanged += OnRenderPropertyValueChanged;
        if (type != null) type.onPropertyChanged += OnTypePropertyValueChanged;
    }
    private void OnInputValueChanged(GUIInput input, object value)
    {
        logger.Info($"输入更新 {input.propertyPath}: {value ?? "null"}({value?.GetType().Name ?? "null"})");
        if (input.isType) {
            type.SetValue(input.propertyPath, value);
        } else if (input.isRender) {
            ConfigRender.SetValue(input.propertyPath, value);
        }
    }

}
