using System;

public class ResourceManager: SingletonMono<ResourceManager>
{
    [Serializable]
    public class ResourceUI
    {
        public UIMWFTypeGun RESOURCE_TYPE_GUN;
    }
    
    public ResourceUI ui = new();
}