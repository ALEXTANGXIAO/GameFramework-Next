using System.Collections.Generic;
using GameFramework.ObjectPool;
using UnityEngine;
using YooAsset;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        private IObjectPool<AssetObject> m_AssetPool;
        
        private readonly Dictionary<UnityEngine.Object,AssetHandle> m_AssetHandleMap = new Dictionary<Object, AssetHandle>();
        
        private readonly Dictionary<string, AssetHandle> m_AssetHandlesCacheMap = new Dictionary<string, AssetHandle>();
    }
}