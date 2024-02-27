using GameFramework;
using GameFramework.ObjectPool;

namespace UnityGameFramework.Runtime
{
    public class AssetItemObject : ObjectBase
    {
        private ResourceComponent m_ResourceComponent;
        
        public static AssetItemObject Create(string location, UnityEngine.Object target, ResourceComponent resourceComponent = null)
        {
            AssetItemObject item = ReferencePool.Acquire<AssetItemObject>();
            item.Initialize(location, target);
            item.m_ResourceComponent = resourceComponent;
            return item;
        }

        protected internal override void Release(bool isShutdown)
        {
            if (Target == null)
            {
                return;
            }
            m_ResourceComponent.UnloadAsset(Target);
            m_ResourceComponent = null;
        }
    }
}