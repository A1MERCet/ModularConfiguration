using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class TexturePBRLoader: SingletonMono<TexturePBRLoader>
{
    public async void LoadAsync(TexutrePBR pbr)
    {
        if (!File.Exists(pbr.baseColorPath)) return;

        byte[] fileData = await File.ReadAllBytesAsync(pbr.baseColorPath);

        await Task.Yield(); 
        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        
        if (texture.LoadImage(fileData)) {
            pbr.baseColor = texture;
        }else {
            Destroy(texture);
        }
        pbr.onLoaded?.Invoke();
    }
}