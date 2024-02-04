using GameFramework.ObjectPool;
using System.Collections.Generic;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        /// <summary>
        /// 资源对象。
        /// </summary>
        private sealed class AssetObject : ObjectBase
        {
            private object m_Resource;
            private ResourceManager m_ResourceManager;

            public AssetObject()
            {
                m_Resource = null;
            }

            public static AssetObject Create(string name, object target, object resource, ResourceManager resourceManager)
            {
                if (resource == null)
                {
                    throw new GameFrameworkException("Resource is invalid.");
                }

                if (resourceManager == null)
                {
                    throw new GameFrameworkException("Resource Manager is invalid.");
                }

                AssetObject assetObject = ReferencePool.Acquire<AssetObject>();
                assetObject.Initialize(name, target);
                assetObject.m_Resource = resource;
                assetObject.m_ResourceManager = resourceManager;
                return assetObject;
            }

            public override void Clear()
            {
                base.Clear();
                m_Resource = null;
            }

            protected internal override void OnUnspawn()
            {
                base.OnUnspawn();
            }

            protected internal override void Release(bool isShutdown)
            {
                if (!isShutdown)
                {
                    // m_ResourceLoader.m_ResourcePool.Unspawn(m_Resource);
                }
                // m_ResourceHelper.Release(Target);
            }
        }
    }
}