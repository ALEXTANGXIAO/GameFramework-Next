using GameFramework.Resource;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityGameFramework.Runtime;
using YooAsset;

namespace GameFramework.Scene
{
    /// <summary>
    /// 场景管理器。
    /// </summary>
    internal sealed class SceneManager : GameFrameworkModule, ISceneManager
    {
        private readonly List<string> m_LoadedSceneAssetNames;
        private readonly List<string> m_LoadingSceneAssetNames;
        private readonly List<string> m_UnloadingSceneAssetNames;
        private readonly LoadSceneCallbacks m_LoadSceneCallbacks;
        private readonly UnloadSceneCallbacks m_UnloadSceneCallbacks;
        private IResourceManager m_ResourceManager;
        private EventHandler<LoadSceneSuccessEventArgs> m_LoadSceneSuccessEventHandler;
        private EventHandler<LoadSceneFailureEventArgs> m_LoadSceneFailureEventHandler;
        private EventHandler<LoadSceneUpdateEventArgs> m_LoadSceneUpdateEventHandler;
        private EventHandler<UnloadSceneSuccessEventArgs> m_UnloadSceneSuccessEventHandler;
        private EventHandler<UnloadSceneFailureEventArgs> m_UnloadSceneFailureEventHandler;

        /// <summary>
        /// 初始化场景管理器的新实例。
        /// </summary>
        public SceneManager()
        {
            m_LoadedSceneAssetNames = new List<string>();
            m_LoadingSceneAssetNames = new List<string>();
            m_UnloadingSceneAssetNames = new List<string>();
            m_LoadSceneCallbacks = new LoadSceneCallbacks(LoadSceneSuccessCallback, LoadSceneFailureCallback, LoadSceneUpdateCallback);
            m_UnloadSceneCallbacks = new UnloadSceneCallbacks(UnloadSceneSuccessCallback, UnloadSceneFailureCallback);
            m_ResourceManager = null;
            m_LoadSceneSuccessEventHandler = null;
            m_LoadSceneFailureEventHandler = null;
            m_LoadSceneUpdateEventHandler = null;
            m_UnloadSceneSuccessEventHandler = null;
            m_UnloadSceneFailureEventHandler = null;
        }

        /// <summary>
        /// 获取游戏框架模块优先级。
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，并且关闭操作会后进行。</remarks>
        internal override int Priority
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// 加载场景成功事件。
        /// </summary>
        public event EventHandler<LoadSceneSuccessEventArgs> LoadSceneSuccess
        {
            add
            {
                m_LoadSceneSuccessEventHandler += value;
            }
            remove
            {
                m_LoadSceneSuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载场景失败事件。
        /// </summary>
        public event EventHandler<LoadSceneFailureEventArgs> LoadSceneFailure
        {
            add
            {
                m_LoadSceneFailureEventHandler += value;
            }
            remove
            {
                m_LoadSceneFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载场景更新事件。
        /// </summary>
        public event EventHandler<LoadSceneUpdateEventArgs> LoadSceneUpdate
        {
            add
            {
                m_LoadSceneUpdateEventHandler += value;
            }
            remove
            {
                m_LoadSceneUpdateEventHandler -= value;
            }
        }

        /// <summary>
        /// 卸载场景成功事件。
        /// </summary>
        public event EventHandler<UnloadSceneSuccessEventArgs> UnloadSceneSuccess
        {
            add
            {
                m_UnloadSceneSuccessEventHandler += value;
            }
            remove
            {
                m_UnloadSceneSuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// 卸载场景失败事件。
        /// </summary>
        public event EventHandler<UnloadSceneFailureEventArgs> UnloadSceneFailure
        {
            add
            {
                m_UnloadSceneFailureEventHandler += value;
            }
            remove
            {
                m_UnloadSceneFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// 关闭并清理场景管理器。
        /// </summary>
        internal override void Shutdown()
        {
            string[] loadedSceneAssetNames = m_LoadedSceneAssetNames.ToArray();
            foreach (string loadedSceneAssetName in loadedSceneAssetNames)
            {
                if (SceneIsUnloading(loadedSceneAssetName))
                {
                    continue;
                }

                UnloadScene(loadedSceneAssetName);
            }

            m_LoadedSceneAssetNames.Clear();
            m_LoadingSceneAssetNames.Clear();
            m_UnloadingSceneAssetNames.Clear();
        }

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        public void SetResourceManager(IResourceManager resourceManager)
        {
            if (resourceManager == null)
            {
                throw new GameFrameworkException("Resource manager is invalid.");
            }

            m_ResourceManager = resourceManager;
        }

        /// <summary>
        /// 获取场景是否已加载。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <returns>场景是否已加载。</returns>
        public bool SceneIsLoaded(string sceneAssetName)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GameFrameworkException("Scene asset name is invalid.");
            }

            return m_LoadedSceneAssetNames.Contains(sceneAssetName);
        }

        /// <summary>
        /// 获取已加载场景的资源名称。
        /// </summary>
        /// <returns>已加载场景的资源名称。</returns>
        public string[] GetLoadedSceneAssetNames()
        {
            return m_LoadedSceneAssetNames.ToArray();
        }

        /// <summary>
        /// 获取已加载场景的资源名称。
        /// </summary>
        /// <param name="results">已加载场景的资源名称。</param>
        public void GetLoadedSceneAssetNames(List<string> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            results.AddRange(m_LoadedSceneAssetNames);
        }

        /// <summary>
        /// 获取场景是否正在加载。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <returns>场景是否正在加载。</returns>
        public bool SceneIsLoading(string sceneAssetName)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GameFrameworkException("Scene asset name is invalid.");
            }

            return m_LoadingSceneAssetNames.Contains(sceneAssetName);
        }

        /// <summary>
        /// 获取正在加载场景的资源名称。
        /// </summary>
        /// <returns>正在加载场景的资源名称。</returns>
        public string[] GetLoadingSceneAssetNames()
        {
            return m_LoadingSceneAssetNames.ToArray();
        }

        /// <summary>
        /// 获取正在加载场景的资源名称。
        /// </summary>
        /// <param name="results">正在加载场景的资源名称。</param>
        public void GetLoadingSceneAssetNames(List<string> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            results.AddRange(m_LoadingSceneAssetNames);
        }

        /// <summary>
        /// 获取场景是否正在卸载。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <returns>场景是否正在卸载。</returns>
        public bool SceneIsUnloading(string sceneAssetName)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GameFrameworkException("Scene asset name is invalid.");
            }

            return m_UnloadingSceneAssetNames.Contains(sceneAssetName);
        }

        /// <summary>
        /// 获取正在卸载场景的资源名称。
        /// </summary>
        /// <returns>正在卸载场景的资源名称。</returns>
        public string[] GetUnloadingSceneAssetNames()
        {
            return m_UnloadingSceneAssetNames.ToArray();
        }

        /// <summary>
        /// 获取正在卸载场景的资源名称。
        /// </summary>
        /// <param name="results">正在卸载场景的资源名称。</param>
        public void GetUnloadingSceneAssetNames(List<string> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            results.AddRange(m_UnloadingSceneAssetNames);
        }

        /// <summary>
        /// 检查场景资源是否存在。
        /// </summary>
        /// <param name="sceneAssetName">要检查场景资源的名称。</param>
        /// <returns>场景资源是否存在。</returns>
        public bool HasScene(string sceneAssetName)
        {
            return m_ResourceManager.HasAsset(sceneAssetName) != HasAssetResult.NotExist;
        }

        /// <summary>
        /// 卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        public void UnloadScene(string sceneAssetName, string packageName = "")
        {
            UnloadScene(sceneAssetName, userData: null, packageName);
        }

        /// <summary>
        /// 卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        public void UnloadScene(string sceneAssetName, object userData, string packageName = "")
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GameFrameworkException("Scene asset name is invalid.");
            }

            if (m_ResourceManager == null)
            {
                throw new GameFrameworkException("You must set resource manager first.");
            }

            if (SceneIsUnloading(sceneAssetName))
            {
                throw new GameFrameworkException(Utility.Text.Format("Scene asset '{0}' is being unloaded.", sceneAssetName));
            }

            if (SceneIsLoading(sceneAssetName))
            {
                throw new GameFrameworkException(Utility.Text.Format("Scene asset '{0}' is being loaded.", sceneAssetName));
            }

            if (!SceneIsLoaded(sceneAssetName))
            {
                throw new GameFrameworkException(Utility.Text.Format("Scene asset '{0}' is not loaded yet.", sceneAssetName));
            }

            m_UnloadingSceneAssetNames.Add(sceneAssetName);
            UnloadScene(sceneAssetName, m_UnloadSceneCallbacks, userData);
        }

        private void LoadSceneSuccessCallback(string sceneAssetName,UnityEngine.SceneManagement.Scene scene, float duration, object userData)
        {
            m_LoadingSceneAssetNames.Remove(sceneAssetName);
            m_LoadedSceneAssetNames.Add(sceneAssetName);
            if (m_LoadSceneSuccessEventHandler != null)
            {
                LoadSceneSuccessEventArgs loadSceneSuccessEventArgs = LoadSceneSuccessEventArgs.Create(sceneAssetName, duration, userData);
                m_LoadSceneSuccessEventHandler(this, loadSceneSuccessEventArgs);
                ReferencePool.Release(loadSceneSuccessEventArgs);
            }
        }

        private void LoadSceneFailureCallback(string sceneAssetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            m_LoadingSceneAssetNames.Remove(sceneAssetName);
            string appendErrorMessage = Utility.Text.Format("Load scene failure, scene asset name '{0}', status '{1}', error message '{2}'.", sceneAssetName, status, errorMessage);
            if (m_LoadSceneFailureEventHandler != null)
            {
                LoadSceneFailureEventArgs loadSceneFailureEventArgs = LoadSceneFailureEventArgs.Create(sceneAssetName, appendErrorMessage, userData);
                m_LoadSceneFailureEventHandler(this, loadSceneFailureEventArgs);
                ReferencePool.Release(loadSceneFailureEventArgs);
                return;
            }

            throw new GameFrameworkException(appendErrorMessage);
        }

        private void LoadSceneUpdateCallback(string sceneAssetName, float progress, object userData)
        {
            if (m_LoadSceneUpdateEventHandler != null)
            {
                LoadSceneUpdateEventArgs loadSceneUpdateEventArgs = LoadSceneUpdateEventArgs.Create(sceneAssetName, progress, userData);
                m_LoadSceneUpdateEventHandler(this, loadSceneUpdateEventArgs);
                ReferencePool.Release(loadSceneUpdateEventArgs);
            }
        }

        private void UnloadSceneSuccessCallback(string sceneAssetName, object userData)
        {
            m_UnloadingSceneAssetNames.Remove(sceneAssetName);
            m_LoadedSceneAssetNames.Remove(sceneAssetName);
            if (m_UnloadSceneSuccessEventHandler != null)
            {
                UnloadSceneSuccessEventArgs unloadSceneSuccessEventArgs = UnloadSceneSuccessEventArgs.Create(sceneAssetName, userData);
                m_UnloadSceneSuccessEventHandler(this, unloadSceneSuccessEventArgs);
                ReferencePool.Release(unloadSceneSuccessEventArgs);
            }
        }

        private void UnloadSceneFailureCallback(string sceneAssetName, object userData)
        {
            m_UnloadingSceneAssetNames.Remove(sceneAssetName);
            if (m_UnloadSceneFailureEventHandler != null)
            {
                UnloadSceneFailureEventArgs unloadSceneFailureEventArgs = UnloadSceneFailureEventArgs.Create(sceneAssetName, userData);
                m_UnloadSceneFailureEventHandler(this, unloadSceneFailureEventArgs);
                ReferencePool.Release(unloadSceneFailureEventArgs);
                return;
            }

            throw new GameFrameworkException(Utility.Text.Format("Unload scene failure, scene asset name '{0}'.", sceneAssetName));
        }

        #region LoadScene
        private string _currentMainSceneName = string.Empty;
        
        private SceneHandle _currentMainScene;
        
        private readonly Dictionary<string,SceneHandle> _subScenes = new Dictionary<string, SceneHandle>();

        /// <summary>
        /// 当前主场景名称。
        /// </summary>
        public string CurrentMainSceneName => _currentMainSceneName; 
        
        /// <summary>
        /// 获取资源定位地址的缓存Key。
        /// </summary>
        /// <param name="location">资源定位地址。</param>
        /// <param name="packageName">资源包名称。</param>
        /// <returns>资源定位地址的缓存Key。</returns>
        private string GetCacheKey(string location, string packageName = "")
        {
            if (string.IsNullOrEmpty(packageName) || packageName.Equals("DefaultPackage"))
            {
                return location;
            }
            return $"{packageName}/{location}";
        }
        
        private SceneHandle GetSceneHandle(string sceneAssetName, LoadSceneMode sceneMode = LoadSceneMode.Single, bool suspendLoad = false, int priority = 100, string packageName = "")
        {
            if (string.IsNullOrEmpty(packageName))
            {
                return YooAssets.LoadSceneAsync(sceneAssetName, sceneMode, suspendLoad, (uint)priority);
            }

            var package = YooAssets.GetPackage(packageName);
            return package.LoadSceneAsync(sceneAssetName, sceneMode, suspendLoad, (uint)priority);
        }
        
        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="sceneMode">场景加载模式</param>
        /// <param name="suspendLoad">加载完毕时是否主动挂起</param>
        /// <param name="priority">加载场景资源的优先级。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        /// <param name="gcCollect">加载场景是否回收垃圾。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        /// <param name="userData">用户自定义数据。</param>
        public async UniTaskVoid LoadScene(string sceneAssetName, LoadSceneCallbacks loadSceneCallbacks = null, LoadSceneMode sceneMode = LoadSceneMode.Single, 
            bool suspendLoad = false, int priority = 100, bool gcCollect = false, string packageName = "", object userData = null)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GameFrameworkException("Scene asset name is invalid.");
            }

            if (m_ResourceManager == null)
            {
                throw new GameFrameworkException("You must set resource manager first.");
            }

            if (SceneIsUnloading(sceneAssetName))
            {
                throw new GameFrameworkException(Utility.Text.Format("Scene asset '{0}' is being unloaded.", sceneAssetName));
            }

            if (SceneIsLoading(sceneAssetName))
            {
                throw new GameFrameworkException(Utility.Text.Format("Scene asset '{0}' is being loaded.", sceneAssetName));
            }

            if (SceneIsLoaded(sceneAssetName))
            {
                throw new GameFrameworkException(Utility.Text.Format("Scene asset '{0}' is already loaded.", sceneAssetName));
            }
            
            float duration = Time.time;

            SceneHandle sceneHandle = null;
            
            if (sceneMode == LoadSceneMode.Additive)
            {
                if (_subScenes.TryGetValue(sceneAssetName, out sceneHandle))
                {
                    Log.Warning($"Could not load subScene while already loaded. Scene: {sceneAssetName}");
                    return;
                }
                sceneHandle = GetSceneHandle(sceneAssetName, sceneMode, suspendLoad, priority, packageName);

                _subScenes.Add(sceneAssetName, sceneHandle);
            }
            else if (sceneMode == LoadSceneMode.Single)
            {
                if (_currentMainScene is { IsDone: false })
                {
                    Log.Warning($"Could not load MainScene while loading. CurrentMainScene: {_currentMainSceneName}.");
                    return;
                }
                sceneHandle = GetSceneHandle(sceneAssetName, sceneMode, suspendLoad, priority, packageName);

                _currentMainSceneName = sceneAssetName;
                
                _currentMainScene = sceneHandle;
                
                GameModule.Resource.ForceUnloadUnusedAssets(gcCollect);
            }
            
            m_LoadingSceneAssetNames.Add(sceneAssetName);
            
            if (sceneHandle is { IsValid: true })
            {
                if (loadSceneCallbacks != null)
                {
                    if (loadSceneCallbacks.LoadSceneSuccessCallback != null)
                    {
                        sceneHandle.Completed += _ =>
                        {
                            duration = Time.time - duration;

                            loadSceneCallbacks.LoadSceneSuccessCallback(sceneAssetName, sceneHandle.SceneObject, duration, userData);
                        };   
                    }
                
                    if (loadSceneCallbacks.LoadSceneUpdateCallback != null)
                    {
                        while (!sceneHandle.IsDone)
                        {
                            await UniTask.Yield();
                
                            loadSceneCallbacks.LoadSceneUpdateCallback?.Invoke(sceneAssetName, sceneHandle.Progress, userData);
                        }
                    }
                }
                
                sceneHandle.Completed += _ =>
                {
                    duration = Time.time - duration;

                    m_LoadSceneCallbacks.LoadSceneSuccessCallback(sceneAssetName, sceneHandle.SceneObject, duration, userData);
                };
                
                while (!sceneHandle.IsDone)
                {
                    await UniTask.Yield();
                
                    m_LoadSceneCallbacks.LoadSceneUpdateCallback?.Invoke(sceneAssetName, sceneHandle.Progress, userData);
                }
            }
        }

        /// <summary>
        /// 异步卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">要卸载场景资源的名称</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        /// <exception cref="GameFrameworkException">游戏框架异常。</exception>
        private void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData = null, string packageName = "")
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GameFrameworkException("Scene asset name is invalid.");
            }

            if (IsMainScene(sceneAssetName))
            {
                if (m_UnloadingSceneAssetNames.Contains(sceneAssetName))
                {
                    m_UnloadingSceneAssetNames.Remove(sceneAssetName);
                }
                return;
            }
            
            _subScenes.TryGetValue(sceneAssetName, out SceneHandle subScene);
            
            if (subScene != null)
            {
                if (subScene.SceneObject == default)
                {
                    Log.Error($"Could not unload Scene while not loaded. Scene: {sceneAssetName}");
                    
                    if (unloadSceneCallbacks is { UnloadSceneFailureCallback: not null })
                    {
                        unloadSceneCallbacks.UnloadSceneFailureCallback(sceneAssetName, userData);
                    }
                    
                    return;
                }
                _subScenes.Remove(sceneAssetName);
                
                UnloadSceneOperation unloadSceneOperation = subScene.UnloadAsync();
                
                unloadSceneOperation.Completed += operation =>
                {
                    if (operation.Status == EOperationStatus.Failed)
                    {
                        if (unloadSceneCallbacks is { UnloadSceneFailureCallback: not null })
                        {
                            unloadSceneCallbacks.UnloadSceneFailureCallback(sceneAssetName, userData);
                        }
                    }
                    else
                    {
                        if (unloadSceneCallbacks is { UnloadSceneSuccessCallback: not null })
                        {
                            unloadSceneCallbacks.UnloadSceneSuccessCallback(sceneAssetName, userData);
                        }
                    }
                };
                return;
            }
            
            if (unloadSceneCallbacks is { UnloadSceneFailureCallback: not null })
            {
                unloadSceneCallbacks.UnloadSceneFailureCallback(sceneAssetName, userData);
            }
            
            Log.Error($"Unload Scene Failed sceneAssetName:{sceneAssetName}");
        }
        #endregion

        #region 操作场景
        /// <summary>
        /// 激活场景（当同时存在多个场景时用于切换激活场景）。
        /// </summary>
        /// <param name="sceneAssetName">场景资源定位地址。</param>
        /// <returns>是否操作成功。</returns>
        public bool ActivateScene(string sceneAssetName)
        {
            if (_currentMainSceneName.Equals(sceneAssetName))
            {
                if (_currentMainScene != null)
                {
                    return _currentMainScene.ActivateScene();
                }
                return false;
            }
            _subScenes.TryGetValue(sceneAssetName, out SceneHandle subScene);
            if (subScene != null)
            {
                return subScene.ActivateScene();
            }
            Log.Warning($"IsMainScene invalid location:{sceneAssetName}");
            return false;
        }

        /// <summary>
        /// 解除场景加载挂起操作。
        /// </summary>
        /// <param name="sceneAssetName">场景资源定位地址。</param>
        /// <returns>是否操作成功。</returns>
        public bool UnSuspend(string sceneAssetName)
        {
            if (_currentMainSceneName.Equals(sceneAssetName))
            {
                if (_currentMainScene != null)
                {
                    return _currentMainScene.UnSuspend();
                }
                return false;
            }
            _subScenes.TryGetValue(sceneAssetName, out SceneHandle subScene);
            if (subScene != null)
            {
                return subScene.UnSuspend();
            }
            Log.Warning($"IsMainScene invalid location:{sceneAssetName}");
            return false;
        }
        
        /// <summary>
        /// 是否为主场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源定位地址。</param>
        /// <returns>是否主场景。</returns>
        public bool IsMainScene(string sceneAssetName)
        {
            if (_currentMainSceneName.Equals(sceneAssetName))
            {
                if (_currentMainScene != null)
                {
                    return _currentMainScene.IsMainScene();
                }
                return true;
            }
            _subScenes.TryGetValue(sceneAssetName, out SceneHandle subScene);
            if (subScene != null)
            {
                return subScene.IsMainScene();
            }
            Log.Warning($"IsMainScene invalid location:{sceneAssetName}");
            return false;
        }
        #endregion
    }
}
