using UnityEngine.UI;

namespace UGFExtensions.Texture
{
    public static partial class SetTextureExtensions
    {
        public static void SetTextureByNetworkAsync(this RawImage rawImage, string file,string saveFilePath = null)
        {
            GameModule.TextureSet.SetTextureByNetworkAsync(SetRawImage.Create(rawImage,file),saveFilePath);
        }
        public static void SetTextureByResourcesAsync(this RawImage rawImage, string file)
        {
            GameModule.TextureSet.SetTextureByResourcesAsync(SetRawImage.Create(rawImage,file));
        }
    }
}