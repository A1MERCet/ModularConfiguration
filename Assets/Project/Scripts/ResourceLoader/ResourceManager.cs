using System;
using System.Collections.Generic;

public class ResourceManager: SingletonMono<ResourceManager>
{
    [Serializable]
    public class ResourceUI
    {
        public UIMWFTypeRender RESOURCE_TYPE_RENDER;
        public UIMWFTypeRender RESOURCE_TYPE_ATT;
        public UIAnimaStageMark RESOURCE_ANIMA_STAGE_MARK;
        public UIAttachSlot RESOURCE_ATTACH_SLOT;
        public SerializableDic<GUIInput.Type, GUIInput> RESOURCE_INPUTS = new();
        public SerializableDic<string, UIConfigProperty> RESOURCE_CONFIGURATION = new();
        public List<ConfigProperty> ACCEPT_CONFIG_PROPERTY = new();
    }
    
    public ResourceUI ui = new();
}