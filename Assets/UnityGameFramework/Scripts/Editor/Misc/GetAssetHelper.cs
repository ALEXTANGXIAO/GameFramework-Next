using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor
{
    /// <summary>
    /// 获取资源路径相关的实用函数。
    /// </summary>
    public static class GetAssetHelper
    {
        [MenuItem("Assets/Get Asset Path", priority = 3)]
        static void GetAssetPath()
        {
            UnityEngine.Object selObj = Selection.activeObject;

            if (selObj != null)
            {
                string assetPath = AssetDatabase.GetAssetPath(selObj);
                EditorGUIUtility.systemCopyBuffer = assetPath;
                Debug.Log($"Asset path is {assetPath}");
            }
        }
    }
}