using GameFramework.ObjectPool;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using YooAsset;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        /// <summary>
        /// 资源对象。
        /// </summary>
        private sealed class AssetObject : ObjectBase
        {
            private object m_AssetHandle;
            private ResourceManager m_ResourceManager;

            public object AssetHandle => m_AssetHandle;

            public AssetObject()
            {
                m_AssetHandle = null;
            }

            public static AssetObject Create(string name, object target, object assetHandle, ResourceManager resourceManager)
            {
                if (assetHandle == null)
                {
                    throw new GameFrameworkException("Resource is invalid.");
                }

                if (resourceManager == null)
                {
                    throw new GameFrameworkException("Resource Manager is invalid.");
                }

                AssetObject assetObject = ReferencePool.Acquire<AssetObject>();
                assetObject.Initialize(name, target);
                assetObject.m_AssetHandle = assetHandle;
                assetObject.m_ResourceManager = resourceManager;
                return assetObject;
            }

            public override void Clear()
            {
                base.Clear();
                m_AssetHandle = null;
            }

            protected internal override void OnUnspawn()
            {
                base.OnUnspawn();
                Log.Warning($"OnUnspawn: {Target} {AssetHandle}");
            }

            protected internal override void Release(bool isShutdown)
            {
                if (!isShutdown)
                {
                    AssetHandle handle = AssetHandle as AssetHandle;
                    Log.Warning($"Release Handle:" + handle);
                    if (handle != null)
                    {
                        handle.Dispose();
                    }
                }
            }
        }
    }
}