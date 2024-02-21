using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameFramework.Resource
{
    public sealed class AssetsReference : MonoBehaviour
    {
        private GameObject _sourceGameObject;
        private List<Object> _refAssetList;
        private IResourceManager _resourceManager;

        private void OnDestroy()
        {
            if (_sourceGameObject != null)
            {
                if (_resourceManager == null)
                {
                    _resourceManager = GameFrameworkEntry.GetModule<IResourceManager>();
                }

                if (_resourceManager != null)
                {
                    _resourceManager.UnloadAsset(_sourceGameObject);

                    if (_refAssetList != null)
                    {
                        foreach (var refAsset in _refAssetList)
                        {
                            _resourceManager.UnloadAsset(refAsset);
                        }
                        _refAssetList.Clear();
                    }
                }
            }
        }

        public AssetsReference Ref(GameObject source, IResourceManager resourceManager = null)
        {
            if (source == null)
            {
                throw new GameFrameworkException($"Source gameObject is null.");
            }

            if (source.scene.name != null)
            {
                throw new GameFrameworkException($"Source gameObject is in scene.");
            }

            _resourceManager = resourceManager;
            _sourceGameObject = source;
            return this;
        }

        public AssetsReference Ref<T>(T source, IResourceManager resourceManager = null) where T : UnityEngine.Object
        {
            if (source == null)
            {
                throw new GameFrameworkException($"Source gameObject is null.");
            }

            _resourceManager = resourceManager;
            _refAssetList = new List<Object>();
            _refAssetList.Add(source);
            return this;
        }

        public static AssetsReference Instantiate(GameObject source, Transform parent = null, IResourceManager resourceManager = null)
        {
            if (source == null)
            {
                throw new GameFrameworkException($"Source gameObject is null.");
            }

            if (source.scene.name != null)
            {
                throw new GameFrameworkException($"Source gameObject is in scene.");
            }

            GameObject instance = Object.Instantiate(source, parent);
            return instance.AddComponent<AssetsReference>().Ref(source, resourceManager);
        }

        public static AssetsReference Ref(GameObject source, GameObject instance, IResourceManager resourceManager = null)
        {
            if (source == null)
            {
                throw new GameFrameworkException($"Source gameObject is null.");
            }

            if (source.scene.name != null)
            {
                throw new GameFrameworkException($"Source gameObject is in scene.");
            }

            return instance.GetOrAddComponent<AssetsReference>().Ref(source, resourceManager);
        }

        public static AssetsReference Ref<T>(T source, GameObject instance, IResourceManager resourceManager = null) where T : UnityEngine.Object
        {
            if (source == null)
            {
                throw new GameFrameworkException($"Source gameObject is null.");
            }

            return instance.GetOrAddComponent<AssetsReference>().Ref(source, resourceManager);
        }
    }
}