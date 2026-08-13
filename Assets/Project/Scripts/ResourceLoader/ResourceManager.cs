using System;

public class ResourceManager: SingletonMono<ResourceManager>
{
    [Serializable]
    public class ResourceUI
    {
        public UIMWFTypeGun RESOURCE_TYPE_GUN;
        public UIAnimaStageMark RESOURCE_ANIMA_STAGE_MARK;
    }
    
    public ResourceUI ui = new();
}