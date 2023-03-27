using GameFramework;
using UGFExtensions.Texture;
using UnityEngine.UI;

/// <summary>
/// 设置图片扩展
/// </summary>
public static partial class SetTextureExtensions
{
    public static void SetTextureByFileSystem(this RawImage rawImage, string file)
    {
        GameModule.TextureSet.SetTextureByFileSystem(SetRawImage.Create(rawImage, file));
    }
    public static void SetTextureByNetwork(this RawImage rawImage, string file, string saveFilePath = null)
    {
        GameModule.TextureSet.SetTextureByNetwork(SetRawImage.Create(rawImage, file), saveFilePath);
    }
    public static void SetTexture(this RawImage rawImage, string file)
    {
        GameModule.TextureSet.SetTextureByResources(SetRawImage.Create(rawImage, file));
    }
}