using UnityEngine.UI;

namespace UGFExtensions.SpriteCollection
{
    public static partial class SetSpriteExtensions
    {
        /// <summary>
        /// 设置精灵
        /// </summary>
        /// <param name="image"></param>
        /// <param name="collectionPath">精灵所在收集器地址</param>
        /// <param name="spritePath">精灵名称</param>
        public static void SetSpriteAsync(this Image image, string collectionPath, string spritePath)
        {
            collectionPath = AssetUtility.UI.GetSpriteCollectionPath(collectionPath);
            spritePath = AssetUtility.UI.GetSpritePath(spritePath);
            GameModule.SpriteCollection.SetSpriteAsync(WaitSetImage.Create(image,collectionPath,spritePath));
        }
    }
}