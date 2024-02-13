using System.Collections.Generic;
using GameFramework.ObjectPool;
using UnityEngine;
using YooAsset;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        private IObjectPool<AssetObject> m_AssetPool;
        
        // private readonly Dictionary<UnityEngine.Object,AssetHandle> m_AssetHandleMap = new Dictionary<Object, AssetHandle>();
        
        private readonly Dictionary<string, AssetHandle> m_AssetHandlesCacheMap = new Dictionary<string, AssetHandle>();
        
        /// <summary>
        /// 获取或设置资源对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        public float AssetAutoReleaseInterval
        {
            get
            {
                return m_AssetPool.AutoReleaseInterval;
            }
            set
            {
                m_AssetPool.AutoReleaseInterval = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的容量。
        /// </summary>
        public int AssetCapacity
        {
            get
            {
                return m_AssetPool.Capacity;
            }
            set
            {
                m_AssetPool.Capacity = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池对象过期秒数。
        /// </summary>
        public float AssetExpireTime
        {
            get
            {
                return m_AssetPool.ExpireTime;
            }
            set
            {
                m_AssetPool.ExpireTime = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的优先级。
        /// </summary>
        public int AssetPriority
        {
            get
            {
                return m_AssetPool.Priority;
            }
            set
            {
                m_AssetPool.Priority = value;
            }
        }
        
        /// <summary>
        /// 获取或设置资源对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        public float ResourceAutoReleaseInterval
        {
            get
            {
                return m_AssetPool.AutoReleaseInterval;
            }
            set
            {
                m_AssetPool.AutoReleaseInterval = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的容量。
        /// </summary>
        public int ResourceCapacity
        {
            get
            {
                return m_AssetPool.Capacity;
            }
            set
            {
                m_AssetPool.Capacity = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池对象过期秒数。
        /// </summary>
        public float ResourceExpireTime
        {
            get
            {
                return m_AssetPool.ExpireTime;
            }
            set
            {
                m_AssetPool.ExpireTime = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的优先级。
        /// </summary>
        public int ResourcePriority
        {
            get
            {
                return m_AssetPool.Priority;
            }
            set
            {
                m_AssetPool.Priority = value;
            }
        }
        
        /// <summary>
        /// 卸载资源。
        /// </summary>
        /// <param name="asset">要卸载的资源。</param>
        public void UnloadAsset(object asset)
        {
            m_AssetPool.Unspawn(asset);
        }
        
        /// <summary>
        /// 设置对象池管理器。
        /// </summary>
        /// <param name="objectPoolManager">对象池管理器。</param>
        public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
        {
            if (objectPoolManager == null)
            {
                throw new GameFrameworkException("Object pool manager is invalid.");
            }
            m_AssetPool = objectPoolManager.CreateMultiSpawnObjectPool<AssetObject>("Asset Pool");
        }
    }
}