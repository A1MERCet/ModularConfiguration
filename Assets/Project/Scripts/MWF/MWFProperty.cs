using System;
using UnityEngine;

[Serializable]
public class MWFProperty
{
    [Serializable]
    public struct Property
    {
        public string key;
        public string lang;
    }

    public SerializableDic<string, Property> properties = new();
    public SerializableDic<string, Color> animaStageColors = new();

    public Property GetProperty(string key) => properties[key];
    public bool HasProperty(string key) => properties.ContainsKey(key);
    public string GetLang(string key) => HasProperty(key) ? properties[key].lang : key;
    public Color GetAnimaStageColor(string animaKey) => animaStageColors.ContainsKey(animaKey) ? animaStageColors[animaKey] : new Color(0.45F, 0.45F, 0.45F);
}