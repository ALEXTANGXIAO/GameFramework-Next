using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.Resource
{
    public static class AssetsSetHelper
    {
        private static IResourceManager _resourceManager;

        private static void CheckResourceManager()
        {
            if (_resourceManager == null)
            {
                _resourceManager = GameFrameworkEntry.GetModule<IResourceManager>();
            }
        }
/*
        #region SetSprite设置图片

        public static void SetSprite(this Image image, string location, bool isAsync = false, string packageName = "")
        {
            if (image == null)
            {
                throw new GameFrameworkException($"SetSprite failed. Because image is null.");
            }

            CheckResourceManager();

            if (!isAsync)
            {
                Sprite sprite = _resourceManager.LoadAsset<Sprite>(location, packageName);
                image.sprite = sprite;
                AssetsReference.Ref(sprite, image.gameObject);
            }
            else
            {
                _resourceManager.LoadAsset<Sprite>(location, sprite =>
                {
                    if (image == null || image.gameObject == null)
                    {
                        _resourceManager.UnloadAsset(sprite);
                        sprite = null;
                        return;
                    }

                    image.sprite = sprite;
                    AssetsReference.Ref(sprite, image.gameObject);
                }, packageName);
            }
        }

        public static void SetSprite(this SpriteRenderer spriteRenderer, string location, bool isAsync = false, string packageName = "")
        {
            if (spriteRenderer == null)
            {
                throw new GameFrameworkException($"SetSprite failed. Because image is null.");
            }

            CheckResourceManager();

            if (!isAsync)
            {
                Sprite sprite = _resourceManager.LoadAsset<Sprite>(location, packageName);
                spriteRenderer.sprite = sprite;
                AssetsReference.Ref(sprite, spriteRenderer.gameObject);
            }
            else
            {
                _resourceManager.LoadAsset<Sprite>(location, sprite =>
                {
                    if (spriteRenderer == null || spriteRenderer.gameObject == null)
                    {
                        _resourceManager.UnloadAsset(sprite);
                        sprite = null;
                        return;
                    }

                    spriteRenderer.sprite = sprite;
                    AssetsReference.Ref(sprite, spriteRenderer.gameObject);
                }, packageName);
            }
        }

        #endregion
*/
        #region SetMaterial

        public static void SetMaterial(this Image image, string location, bool isAsync = false, string packageName = "")
        {
            if (image == null)
            {
                throw new GameFrameworkException($"SetSprite failed. Because image is null.");
            }

            CheckResourceManager();

            if (!isAsync)
            {
                Material material = _resourceManager.LoadAsset<Material>(location, packageName);
                image.material = material;
                AssetsReference.Ref(material, image.gameObject);
            }
            else
            {
                _resourceManager.LoadAsset<Material>(location, material =>
                {
                    if (image == null || image.gameObject == null)
                    {
                        _resourceManager.UnloadAsset(material);
                        material = null;
                        return;
                    }

                    image.material = material;
                    AssetsReference.Ref(material, image.gameObject);
                }, packageName);
            }
        }

        public static void SetMaterial(this SpriteRenderer spriteRenderer, string location, bool isAsync = false, string packageName = "")
        {
            if (spriteRenderer == null)
            {
                throw new GameFrameworkException($"SetSprite failed. Because image is null.");
            }

            CheckResourceManager();

            if (!isAsync)
            {
                Material material = _resourceManager.LoadAsset<Material>(location, packageName);
                spriteRenderer.material = material;
                AssetsReference.Ref(material, spriteRenderer.gameObject);
            }
            else
            {
                _resourceManager.LoadAsset<Material>(location, material =>
                {
                    if (spriteRenderer == null || spriteRenderer.gameObject == null)
                    {
                        _resourceManager.UnloadAsset(material);
                        material = null;
                        return;
                    }

                    spriteRenderer.material = material;
                    AssetsReference.Ref(material, spriteRenderer.gameObject);
                }, packageName);
            }
        }

        public static void SetMaterial(this MeshRenderer meshRenderer, string location, bool needInstance = true, bool isAsync = false, string packageName = "")
        {
            if (meshRenderer == null)
            {
                throw new GameFrameworkException($"SetSprite failed. Because image is null.");
            }

            CheckResourceManager();

            if (!isAsync)
            {
                Material material = _resourceManager.LoadAsset<Material>(location, packageName);
                meshRenderer.material = needInstance ? Object.Instantiate(material) : material;
                AssetsReference.Ref(material, meshRenderer.gameObject);
            }
            else
            {
                _resourceManager.LoadAsset<Material>(location, material =>
                {
                    if (meshRenderer == null || meshRenderer.gameObject == null)
                    {
                        _resourceManager.UnloadAsset(material);
                        material = null;
                        return;
                    }

                    meshRenderer.material = needInstance ? Object.Instantiate(material) : material;
                    AssetsReference.Ref(material, meshRenderer.gameObject);
                }, packageName);
            }
        }

        public static void SetSharedMaterial(this MeshRenderer meshRenderer, string location, bool isAsync = false, string packageName = "")
        {
            if (meshRenderer == null)
            {
                throw new GameFrameworkException($"SetSprite failed. Because image is null.");
            }

            CheckResourceManager();

            if (!isAsync)
            {
                Material material = _resourceManager.LoadAsset<Material>(location, packageName);
                meshRenderer.sharedMaterial = material;
                AssetsReference.Ref(material, meshRenderer.gameObject);
            }
            else
            {
                _resourceManager.LoadAsset<Material>(location, material =>
                {
                    if (meshRenderer == null || meshRenderer.gameObject == null)
                    {
                        _resourceManager.UnloadAsset(material);
                        material = null;
                        return;
                    }

                    meshRenderer.sharedMaterial = material;
                    AssetsReference.Ref(material, meshRenderer.gameObject);
                }, packageName);
            }
        }

        #endregion
    }
}