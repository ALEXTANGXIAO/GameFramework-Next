using UGFExtensions.Await;
using UnityEngine;

namespace UGFExtensions.Texture
{
    public partial class TextureSetComponent
    {
        /// <summary>
        /// 通过资源系统设置图片
        /// </summary>
        /// <param name="setTexture2dObject">需要设置图片的对象</param>
        public async void SetTextureByResourcesAsync(ISetTexture2dObject setTexture2dObject)
        {
            Texture2D texture;
            if (m_TexturePool.CanSpawn(setTexture2dObject.Texture2dFilePath))
            {
                texture = (Texture2D)m_TexturePool.Spawn(setTexture2dObject.Texture2dFilePath).Target;
            }
            else
            {
                texture = await GameModule.Resource.LoadAssetAsync<Texture2D>(setTexture2dObject.Texture2dFilePath);
                m_TexturePool.Register(TextureItemObject.Create(setTexture2dObject.Texture2dFilePath, texture,TextureLoad.FromResource,m_ResourceComponent), true);
            }

            SetTexture(setTexture2dObject, texture);
        }
    }
}