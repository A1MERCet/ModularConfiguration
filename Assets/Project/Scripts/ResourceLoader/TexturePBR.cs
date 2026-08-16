using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class TexturePBR
{
    public string baseColorPath;
    public Texture2D baseColor;

    public Action onLoaded;

    public async void LoadAsync()
    {
        TexturePBRLoader.instance.LoadAsync(this);
    }
}