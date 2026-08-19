using System;
using System.Collections.Generic;

[Serializable]
public class ConfigProperty
{
    public List<string> acceptTypes = new();
    public SerializableDic<string, GUIInput.Type> property = new();
}