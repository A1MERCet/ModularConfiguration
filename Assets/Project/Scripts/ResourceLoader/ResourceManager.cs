using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
        public RenderTexture RESOURCE_RT_SCOPE;
    }
    
    public ResourceUI ui = new();
}