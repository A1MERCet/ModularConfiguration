public struct ModelSkin
{ 
    public string internalName;
    public string skinAsset;
    public string displayName;
    public string[] textures;

    public override string ToString() => $"Name: {internalName}/{displayName} SkinAsset: {skinAsset} Textures: {textures}";
}