using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityGameFramework.Resource;
using UnityGameFramework.Runtime;
using YooAsset;

namespace GameFramework.Resource
{
    /// <summary>
    /// 资源管理器。
    /// </summary>
    internal sealed partial class ResourceManager : GameFrameworkModule,IResourceManager
    {
        #region Propreties
        /// <summary>
        /// 资源包名称。
        /// </summary>
        public string PackageName { get; set; } = "DefaultPackage";
        
        /// <summary>
        /// 资源系统运行模式。
        /// </summary>
        public EPlayMode PlayMode { get; set; }

        /// <summary>
        /// 下载文件校验等级。
        /// </summary>
        public EVerifyLevel VerifyLevel { get; set; }

        /// <summary>
        /// 设置异步系统参数，每帧执行消耗的最大时间切片（单位：毫秒）
        /// </summary>
        public long Milliseconds { get; set; }

        /// <summary>
        /// 获取游戏框架模块优先级。
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，并且关闭操作会后进行。</remarks>
        internal override int Priority => 4;

        /// <summary>
        /// 实例化的根节点。
        /// </summary>
        public Transform InstanceRoot { get; set; }
        
        /// <summary>
        /// 资源生命周期服务器。
        /// </summary>
        public ResourceHelper ResourceHelper { get;private set; }
        
        /// <summary>
        /// Propagates notification that operations should be canceled.
        /// </summary>
        public CancellationToken CancellationToken { get;private set; }

        /// <summary>
        /// 资源服务器地址。
        /// </summary>
        public string HostServerURL { get; set; }

        /// <summary>
        /// The total number of frames since the start of the game (Read Only).
        /// </summary>
        private static int _lastUpdateFrame = 0;
        
        private string m_ApplicableGameVersion;
        
        private int m_InternalResourceVersion;
        
        private string m_ReadOnlyPath;
        private string m_ReadWritePath;

        /// <summary>
        /// 获取资源只读区路径。
        /// </summary>
        public string ReadOnlyPath
        {
            get
            {
                return m_ReadOnlyPath;
            }
        }

        /// <summary>
        /// 获取资源读写区路径。
        /// </summary>
        public string ReadWritePath
        {
            get
            {
                return m_ReadWritePath;
            }
        }

        /// <summary>
        /// 获取当前资源适用的游戏版本号。
        /// </summary>
        public string ApplicableGameVersion
        {
            get
            {
                return m_ApplicableGameVersion;
            }
        }
        
        /// <summary>
        /// 获取当前内部资源版本号。
        /// </summary>
        public int InternalResourceVersion
        {
            get
            {
                return m_InternalResourceVersion;
            }
        }

        public int DownloadingMaxNum { get; set; }
        public int FailedTryAgain { get; set; }

        #endregion

        /// <summary>
        /// 初始化资源管理器的新实例。
        /// </summary>
        public ResourceManager()
        {
            
        }

        public void Initialize()
        {
            // 初始化资源系统
            YooAssets.Initialize(new YooAssetsLogger(), InstanceRoot);
            YooAssets.SetOperationSystemMaxTimeSlice(Milliseconds);
            YooAssets.SetCacheSystemCachedFileVerifyLevel(VerifyLevel);

            // 创建默认的资源包
            string packageName = PackageName;
            var defaultPackage = YooAssets.TryGetPackage(packageName);
            if (defaultPackage == null)
            {
                defaultPackage = YooAssets.CreatePackage(packageName);
                YooAssets.SetDefaultPackage(defaultPackage);
            }
            ResourceHelper = InstanceRoot.gameObject.AddComponent<ResourceHelper>();
            CancellationToken = ResourceHelper.GetCancellationTokenOnDestroy();
        }

        #region 设置接口
        /// <summary>
        /// 设置资源只读区路径。
        /// </summary>
        /// <param name="readOnlyPath">资源只读区路径。</param>
        public void SetReadOnlyPath(string readOnlyPath)
        {
            if (string.IsNullOrEmpty(readOnlyPath))
            {
                throw new GameFrameworkException("Read-only path is invalid.");
            }

            m_ReadOnlyPath = readOnlyPath;
        }

        /// <summary>
        /// 设置资源读写区路径。
        /// </summary>
        /// <param name="readWritePath">资源读写区路径。</param>
        public void SetReadWritePath(string readWritePath)
        {
            if (string.IsNullOrEmpty(readWritePath))
            {
                throw new GameFrameworkException("Read-write path is invalid.");
            }

            m_ReadWritePath = readWritePath;
        }
        

        #endregion

        public InitializationOperation InitPackage()
        {
            // 创建默认的资源包
            string packageName = PackageName;
            var package = YooAssets.TryGetPackage(packageName);
            if (package == null)
            {
                package = YooAssets.CreatePackage(packageName);
                YooAssets.SetDefaultPackage(package);
            }

            // 编辑器下的模拟模式
            InitializationOperation initializationOperation = null;
            if (PlayMode == EPlayMode.EditorSimulateMode)
            {
                var createParameters = new EditorSimulateModeParameters();
                createParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(packageName);
                initializationOperation = package.InitializeAsync(createParameters);
            }

            // 单机运行模式
            if (PlayMode == EPlayMode.OfflinePlayMode)
            {
                var createParameters = new OfflinePlayModeParameters();
                createParameters.DecryptionServices = new GameDecryptionServices();
                initializationOperation = package.InitializeAsync(createParameters);
            }

            // 联机运行模式
            if (PlayMode == EPlayMode.HostPlayMode)
            {
                var createParameters = new HostPlayModeParameters();
                createParameters.DecryptionServices = new GameDecryptionServices();
                createParameters.QueryServices = new GameQueryServices();
                createParameters.DefaultHostServer = HostServerURL;
                createParameters.FallbackHostServer = HostServerURL;
                initializationOperation = package.InitializeAsync(createParameters);
            }

            return initializationOperation;
        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            DebugCheckDuplicateDriver();
            YooAssets.Update();
        }

        internal override void Shutdown()
        {
            YooAssets.Destroy();
        }

        [Conditional("DEBUG")]
        private void DebugCheckDuplicateDriver()
        {
            if (_lastUpdateFrame > 0)
            {
                if (_lastUpdateFrame == Time.frameCount)
                    YooLogger.Warning($"There are two {nameof(YooAssetsDriver)} in the scene. Please ensure there is always exactly one driver in the scene.");
            }

            _lastUpdateFrame = Time.frameCount;
        }

        #region Public Methods

        /// <summary>
        /// 设置默认的资源包。
        /// </summary>
        public void SetDefaultPackage(ResourcePackage package)
        {
            YooAssets.SetDefaultPackage(package);
        }

        #region 资源信息

        /// <summary>
        /// 是否需要从远端更新下载。
        /// </summary>
        /// <param name="location">资源的定位地址</param>
        public bool IsNeedDownloadFromRemote(string location)
        {
            return YooAssets.IsNeedDownloadFromRemote(location);
        }

        /// <summary>
        /// 是否需要从远端更新下载。
        /// </summary>
        /// <param name="assetInfo">资源信息。</param>
        public bool IsNeedDownloadFromRemote(AssetInfo assetInfo)
        {
            return YooAssets.IsNeedDownloadFromRemote(assetInfo);
        }

        /// <summary>
        /// 获取资源信息列表。
        /// </summary>
        /// <param name="tag">资源标签。</param>
        /// <returns>资源信息列表。</returns>
        public AssetInfo[] GetAssetInfos(string tag)
        {
            return YooAssets.GetAssetInfos(tag);
        }

        /// <summary>
        /// 获取资源信息列表。
        /// </summary>
        /// <param name="tags">资源标签列表。</param>
        /// <returns>资源信息列表。</returns>
        public AssetInfo[] GetAssetInfos(string[] tags)
        {
            return YooAssets.GetAssetInfos(tags);
        }

        /// <summary>
        /// 获取资源信息。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <returns>资源信息。</returns>
        public AssetInfo GetAssetInfo(string location)
        {
            return YooAssets.GetAssetInfo(location);
        }

        /// <summary>
        /// 检查资源定位地址是否有效。
        /// </summary>
        /// <param name="location">资源的定位地址</param>
        public bool CheckLocationValid(string location)
        {
            return YooAssets.CheckLocationValid(location);
        }

        #endregion

        #region 资源加载

        /// <summary>
        /// 同步加载资源对象
        /// </summary>
        /// <param name="assetInfo">资源信息</param>
        public AssetOperationHandle LoadAssetSync(AssetInfo assetInfo)
        {
            return YooAssets.LoadAssetSync(assetInfo);
        }

        /// <summary>
        /// 同步加载资源对象
        /// </summary>
        /// <typeparam name="TObject">资源类型</typeparam>
        /// <param name="location">资源的定位地址</param>
        public AssetOperationHandle LoadAssetSync<TObject>(string location) where TObject : UnityEngine.Object
        {
            return YooAssets.LoadAssetSync<TObject>(location);
        }

        /// <summary>
        /// 同步加载资源对象
        /// </summary>
        /// <param name="location">资源的定位地址</param>
        /// <param name="type">资源类型</param>
        public AssetOperationHandle LoadAssetSync(string location, System.Type type)
        {
            return YooAssets.LoadAssetSync(location, type);
        }


        /// <summary>
        /// 异步加载资源对象
        /// </summary>
        /// <param name="assetInfo">资源信息</param>
        public AssetOperationHandle LoadAssetAsync(AssetInfo assetInfo)
        {
            return YooAssets.LoadAssetAsync(assetInfo);
        }

        /// <summary>
        /// 异步加载资源对象
        /// </summary>
        /// <typeparam name="TObject">资源类型</typeparam>
        /// <param name="location">资源的定位地址</param>
        public AssetOperationHandle LoadAssetAsync<TObject>(string location) where TObject : UnityEngine.Object
        {
            return YooAssets.LoadAssetAsync<TObject>(location);
        }

        /// <summary>
        /// 异步加载资源对象
        /// </summary>
        /// <param name="location">资源的定位地址</param>
        /// <param name="type">资源类型</param>
        public AssetOperationHandle LoadAssetAsync(string location, System.Type type)
        {
            return YooAssets.LoadAssetAsync(location, type);
        }

        /// <summary>
        /// 同步加载资源并获取句柄。
        /// </summary>
        /// <param name="location">要加载资源的名称。</param>
        /// <typeparam name="T">要加载资源的类型。</typeparam>
        /// <returns>同步加载资源句柄。</returns>
        public AssetOperationHandle LoadAssetGetOperation<T>(string location) where T : UnityEngine.Object
        {
            var handle = LoadAssetSync<T>(location);

            return handle;
        }
        
        /// <summary>
        /// 同步加载资源对象。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <typeparam name="T">资源类型。</typeparam>
        /// <returns>资源实例。</returns>
        public T LoadAsset<T>(string location) where T : UnityEngine.Object
        {
            var assetPackage =  YooAssets.TryGetPackage(PackageName);
            
            AssetInfo assetInfo = assetPackage.GetAssetInfo(location);

            if (assetInfo == null)
            {
                string errorMessage = Utility.Text.Format("Can not load asset '{0}'.", location);

                throw new GameFrameworkException(errorMessage);
            }

            var handle = LoadAssetSync<T>(location);

            if (typeof(T) == typeof(UnityEngine.GameObject))
            {
                return handle.InstantiateSync() as T;
            }

            return handle.AssetObject as T;
        }
        
        /// <summary>
        /// 同步加载资源对象。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <param name="parent">父节点。</param>
        /// <typeparam name="TObject">资源类型。</typeparam>
        /// <returns>资源实例。</returns>
        public TObject LoadAsset<TObject>(string location,Transform parent) where TObject : UnityEngine.Object
        {
            var handle = LoadAssetSync<TObject>(location);

            if (typeof(TObject) == typeof(UnityEngine.GameObject))
            {
                return handle.InstantiateSync(parent) as TObject;
            }

            return handle.AssetObject as TObject;
        }

        public async UniTask<TObject> LoadAsync<TObject>(string location) where TObject : UnityEngine.Object
        {
            var assetPackage =  YooAssets.TryGetPackage(PackageName);
            
            var handle = assetPackage.LoadAssetAsync<TObject>(location);

            await handle.ToUniTask(ResourceHelper);

            return handle.AssetObject as TObject;
        }
        #endregion
        
        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public async void LoadAssetAsync(string assetName, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData)
        {
            float duration = Time.time;
            
            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            if (loadAssetCallbacks == null)
            {
                throw new GameFrameworkException("Load asset callbacks is invalid.");
            }
            
            var assetPackage =  YooAssets.TryGetPackage(PackageName);
            
            AssetInfo assetInfo = assetPackage.GetAssetInfo(assetName);

            if (assetInfo == null)
            {
                string errorMessage = Utility.Text.Format("Can not load asset '{0}'.", assetName);
                if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                {
                    loadAssetCallbacks.LoadAssetFailureCallback(assetName, LoadResourceStatus.NotExist, errorMessage, userData);
                    return;
                }

                throw new GameFrameworkException(errorMessage);
            }

            OperationHandleBase handleBase;

            handleBase = assetPackage.LoadAssetAsync(assetName, assetType);
            
            await handleBase.ToUniTask(ResourceHelper);

            AssetOperationHandle handle = (AssetOperationHandle)handleBase;
            if (handle == null || handle.AssetObject == null || handle.Status == EOperationStatus.Failed)
            {
                string errorMessage = Utility.Text.Format("Can not load asset '{0}'.", assetName);
                if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                {
                    loadAssetCallbacks.LoadAssetFailureCallback(assetName, LoadResourceStatus.NotReady, errorMessage, userData);
                    return;
                }

                throw new GameFrameworkException(errorMessage);
            }
            else
            {
                if (loadAssetCallbacks.LoadAssetSuccessCallback != null)
                {
                    duration = Time.time - duration;
                    
                    loadAssetCallbacks.LoadAssetSuccessCallback(assetName, handle.AssetObject, duration, userData);
                }
            }
        }

        
        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public async void LoadAssetAsync(string assetName, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData)
        {
            float duration = Time.time;
            
            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            if (loadAssetCallbacks == null)
            {
                throw new GameFrameworkException("Load asset callbacks is invalid.");
            }
            
            var assetPackage =  YooAssets.TryGetPackage(PackageName);
            
            AssetInfo assetInfo = assetPackage.GetAssetInfo(assetName);

            if (assetInfo == null)
            {
                string errorMessage = Utility.Text.Format("Can not load asset '{0}'.", assetName);
                if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                {
                    loadAssetCallbacks.LoadAssetFailureCallback(assetName, LoadResourceStatus.NotExist, errorMessage, userData);
                    return;
                }

                throw new GameFrameworkException(errorMessage);
            }

            OperationHandleBase handleBase;

            handleBase = assetPackage.LoadAssetAsync(assetInfo);
            
            await handleBase.ToUniTask(ResourceHelper);

            AssetOperationHandle handle = (AssetOperationHandle)handleBase;
            if (handle == null || handle.AssetObject == null || handle.Status == EOperationStatus.Failed)
            {
                string errorMessage = Utility.Text.Format("Can not load asset '{0}'.", assetName);
                if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                {
                    loadAssetCallbacks.LoadAssetFailureCallback(assetName, LoadResourceStatus.NotReady, errorMessage, userData);
                    return;
                }

                throw new GameFrameworkException(errorMessage);
            }
            else
            {
                if (loadAssetCallbacks.LoadAssetSuccessCallback != null)
                {
                    duration = Time.time - duration;
                
                    loadAssetCallbacks.LoadAssetSuccessCallback(assetName, handle.AssetObject, duration, userData);
                }
            }
        }
        #endregion
        
        public void UnloadUnusedAssets()
        {
            YooAssets.UnloadUnusedAssets();
        }
        
        public void ForceUnloadAllAssets()
        {
            YooAssets.ForceUnloadAllAssets();
        }
        
        public void UnloadAsset(object asset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 检查资源是否存在。
        /// </summary>
        /// <param name="assetName">要检查资源的名称。</param>
        /// <returns>检查资源是否存在的结果。</returns>
        public HasAssetResult HasAsset(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

#if false
            UnityEngine.Object obj = UnityEditor.AssetDatabase.LoadMainAssetAtPath(assetName);
            if (obj == null)
            {
                return HasAssetResult.NotExist;
            }

            HasAssetResult result = obj.GetType() == typeof(UnityEditor.DefaultAsset) ? HasAssetResult.BinaryOnDisk : HasAssetResult.AssetOnDisk;
            obj = null;
            UnityEditor.EditorUtility.UnloadUnusedAssetsImmediate();
            return result;
#else
            AssetInfo assetInfo = YooAssets.GetAssetInfo(assetName);
            
            if (assetInfo == null)
            {
                return HasAssetResult.NotExist;
            }

            return HasAssetResult.AssetOnDisk;
#endif
        }

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="priority">加载场景资源的优先级。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public async void LoadScene(string sceneAssetName, int priority, LoadSceneCallbacks loadSceneCallbacks, object userData = null)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GameFrameworkException("Scene asset name is invalid.");
            }

            if (loadSceneCallbacks == null)
            {
                throw new GameFrameworkException("Load scene callbacks is invalid.");
            }
            
            float duration = Time.time;

            SceneOperationHandle handle = YooAssets.LoadSceneAsync(sceneAssetName,LoadSceneMode.Single,activateOnLoad:true,priority:priority);

            await handle.ToUniTask(ResourceHelper);
            
            if (loadSceneCallbacks.LoadSceneSuccessCallback != null)
            {
                duration = Time.time - duration;
                    
                loadSceneCallbacks.LoadSceneSuccessCallback(sceneAssetName, handle.SceneObject, duration, userData);
            }
        }

        /// <summary>
        /// 异步卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">要卸载场景资源的名称</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <exception cref="GameFrameworkException">游戏框架异常。</exception>
        public void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData = null)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GameFrameworkException("Scene asset name is invalid.");
            }

            if (unloadSceneCallbacks == null)
            {
                throw new GameFrameworkException("Unload scene callbacks is invalid.");
            }
            
            Utility.Unity.StartCoroutine(UnloadSceneCo(sceneAssetName, unloadSceneCallbacks, userData));
        }
        
        private IEnumerator UnloadSceneCo(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
        {
            AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(SceneComponent.GetSceneName(sceneAssetName));
            if (asyncOperation == null)
            {
                yield break;
            }

            yield return asyncOperation;

            if (asyncOperation.allowSceneActivation)
            {
                if (unloadSceneCallbacks.UnloadSceneSuccessCallback != null)
                {
                    unloadSceneCallbacks.UnloadSceneSuccessCallback(sceneAssetName, userData);
                }
            }
            else
            {
                if (unloadSceneCallbacks.UnloadSceneFailureCallback != null)
                {
                    unloadSceneCallbacks.UnloadSceneFailureCallback(sceneAssetName, userData);
                }
            }
        }
    }
}