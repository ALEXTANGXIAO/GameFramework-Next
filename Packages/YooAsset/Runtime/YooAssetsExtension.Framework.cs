using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace YooAsset
{
    public static partial class YooAssets
    {
        #region 拓展
        /// <summary>
        /// 获取文件的CRC32
        /// </summary>
        public static string GetFileCRC32(string filePath)
        {
            try
            {
                return HashUtility.FileCRC32(filePath);
            }
            catch (Exception e)
            {
                YooLogger.Exception(e);
                return string.Empty;
            }
        }
        #endregion
    }
}