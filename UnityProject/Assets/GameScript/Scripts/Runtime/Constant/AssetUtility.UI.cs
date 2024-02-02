using System.Collections;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;

/// <summary>
/// 资源路径相关
/// </summary>
public static partial class AssetUtility
{
    /// <summary>
    /// UI相关
    /// </summary>
    public static class UI
    {
        /// <summary>
        /// 获取精灵资源名称
        /// </summary>
        /// <param name="spriteName"></param>
        /// <returns></returns>
        public static string GetSpritePath(string spriteName)
        {
            return $"Assets/GameMain/ArtRaw/UIRaw/{spriteName}";
        }

        /// <summary>
        /// 获取精灵资源收集器
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public static string GetSpriteCollectionPath(string collectionName)
        {
            return $"Assets/GameMain/ArtRaw/AtlasCollection/{collectionName}.asset";
        }

        /// <summary>
        /// 获取大图
        /// </summary>
        /// <param name="textureName"></param>
        /// <returns></returns>
        public static string GetTexturePath(string textureName)
        {
            return $"Assets/GameMain/ArtRaw/Texture/{textureName}.png";
        }

        /// <summary>
        /// 获取大图
        /// </summary>
        /// <param name="textureName"></param>
        /// <returns></returns>
        public static string GetRenderTexturePath(string textureName)
        {
            return $"Assets/GameMain/ArtRaw/Texture/{textureName}.renderTexture";
        }

        /// <summary>
        /// 获取UI资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static string GetUIFormAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/AssetsHotfix/UI/UIForms/{0}.prefab", assetName);
        }
    }
}