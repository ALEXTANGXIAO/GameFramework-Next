using System;
using UnityEngine;
using YooAsset;

namespace GameFramework.Resource
{
    /// <summary>
    /// 资源管理器接口。
    /// </summary>
    public interface IResourceManager
    {
        /// <summary>
        /// 获取当前资源适用的游戏版本号。
        /// </summary>
        string ApplicableGameVersion
        {
            get;
        }

        /// <summary>
        /// 获取当前内部资源版本号。
        /// </summary>
        int InternalResourceVersion
        {
            get;
        }

        /// <summary>
        /// 同时下载的最大数目。
        /// </summary>
        int DownloadingMaxNum
        {
            get;
            set;
        }
        
        /// <summary>
        /// 失败重试最大数目。
        /// </summary>
        int FailedTryAgain
        {
            get;
            set;
        }
        
        /// <summary>
        /// 获取资源只读区路径。
        /// </summary>
        string ReadOnlyPath
        {
            get;
        }

        /// <summary>
        /// 获取资源读写区路径。
        /// </summary>
        string ReadWritePath
        {
            get;
        }
        
        /// <summary>
        /// 设置资源只读区路径。
        /// </summary>
        /// <param name="readOnlyPath">资源只读区路径。</param>
        void SetReadOnlyPath(string readOnlyPath);

        /// <summary>
        /// 设置资源读写区路径。
        /// </summary>
        /// <param name="readWritePath">资源读写区路径。</param>
        void SetReadWritePath(string readWritePath);

        /// <summary>
        /// 初始化接口。
        /// </summary>
        void Initialize();

        /// <summary>
        /// 初始化操作。
        /// </summary>
        /// <returns></returns>
        InitializationOperation InitPackage();
        
        /// <summary>
        /// 获取或设置资源包名称。
        /// </summary>
        string PackageName
        {
            get;
            set;
        }
        
        /// <summary>
        /// 获取或设置运行模式。
        /// </summary>
        EPlayMode PlayMode
        {
            get;
            set;
        }
        
        /// <summary>
        /// 获取或设置下载文件校验等级。
        /// </summary>
        EVerifyLevel VerifyLevel
        {
            get;
            set;
        }
        
        /// <summary>
        /// 获取或设置异步系统参数，每帧执行消耗的最大时间切片（单位：毫秒）。
        /// </summary>
        long Milliseconds
        {
            get;
            set;
        }
        
        Transform InstanceRoot
        {
            get;
            set;
        }
        
        /// <summary>
        /// 热更链接URL。
        /// </summary>
        string HostServerURL
        {
            get;
            set;
        }
        
        /// <summary>
        /// 卸载资源。
        /// </summary>
        /// <param name="asset">要卸载的资源。</param>
        void UnloadAsset(object asset);

        /// <summary>
        /// 资源回收（卸载引用计数为零的资源）
        /// </summary>
        void UnloadUnusedAssets();
        
        /// <summary>
        /// 强制回收所有资源
        /// </summary>
        void ForceUnloadAllAssets();

        /// <summary>
        /// 同步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <typeparam name="T">要加载资源的类型。</typeparam>
        /// <returns>资源实例。</returns>
        T LoadAsset<T>(string assetName) where T : UnityEngine.Object;
        
        /// <summary>
        /// 同步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="parent">父节点位置。</param>
        /// <typeparam name="T">要加载资源的类型。</typeparam>
        /// <returns>资源实例。</returns>
        T LoadAsset<T>(string assetName,Transform parent) where T : UnityEngine.Object;
        
        /// <summary>
        /// 异步加载资源并获取句柄。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <typeparam name="T">要加载资源的类型。</typeparam>
        /// <returns>同步加载资源句柄。</returns>
        AssetOperationHandle LoadAssetAsync<T>(string assetName) where T : UnityEngine.Object;
        
        /// <summary>
        /// 同步加载资源并获取句柄。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <typeparam name="T">要加载资源的类型。</typeparam>
        /// <returns>同步加载资源句柄。</returns>
        AssetOperationHandle LoadAssetGetOperation<T>(string assetName) where T : UnityEngine.Object;
        
        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadAssetAsync(string assetName, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData);
        
        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载的资源类型。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadAssetAsync(string assetName,Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData);
        
        /// <summary>
        /// 检查资源是否存在。
        /// </summary>
        /// <param name="assetName">要检查资源的名称。</param>
        /// <returns>检查资源是否存在的结果。</returns>
        HasAssetResult HasAsset(string assetName);
        
        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="priority">加载场景资源的优先级。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadScene(string sceneAssetName, int priority, LoadSceneCallbacks loadSceneCallbacks, object userData = null);
        /// <summary>
        /// 异步卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">要卸载场景资源的名称。</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData = null);
    }
}