using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        public sealed class StreamingAssetsHelper
        {
            private static readonly Dictionary<string, bool> _cacheData = new Dictionary<string, bool>(1000);

#if UNITY_ANDROID && !UNITY_EDITOR
            private static AndroidJavaClass _unityPlayerClass;

            public static AndroidJavaClass UnityPlayerClass
            {
                get
                {
                    if (_unityPlayerClass == null)
                        _unityPlayerClass = new UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    return _unityPlayerClass;
                }
            }

            private static AndroidJavaObject _currentActivity;

            public static AndroidJavaObject CurrentActivity
            {
                get
                {
                    if (_currentActivity == null)
                        _currentActivity = UnityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
                    return _currentActivity;
                }
            }
            
            private static AndroidJavaObject _assetManager;

            public static AndroidJavaObject AssetManager
            {
                get
                {
                    if (_assetManager == null)
                        _assetManager = CurrentActivity.Call<AndroidJavaObject>("GetAssets");;
                    return _assetManager;
                }
            }
            
            /// <summary>
            /// 利用安卓原生接口查询内置文件是否存在
            /// </summary>
            public static bool FileExists(string filePath)
            {
                if (_cacheData.TryGetValue(filePath, out bool result) == false)
                {
                    result = CurrentActivity.Call<bool>("checkAssetExist", filePath);
                    _cacheData.Add(filePath, result);
                }

                Log.Warning($"FileExists ? :{filePath} result:{result}");

                return result;
            }
#else
            public static bool FileExists(string filePath)
            {
                string path = string.Empty;

                if (_cacheData.TryGetValue(filePath, out bool result) == false)
                {
                    path = System.IO.Path.Combine(Application.streamingAssetsPath, filePath);
                    result = System.IO.File.Exists(path);
                    _cacheData.Add(filePath, result);
                }
                
                Log.Warning($"FileExists ? :{path} result:{result}");

                return result;
            }
#endif
        }
    }
}