using System;
using YooAsset;

namespace UnityGameFramework.Runtime
{
    public class YooAssetsLogger:ILogger
    {
        public void Log(string message)
        {
            Runtime.Log.Info(message);
        }

        public void Warning(string message)
        {
            Runtime.Log.Warning(message);
        }

        public void Error(string message)
        {
            Runtime.Log.Error(message);
        }

        public void Exception(Exception exception)
        {
            Runtime.Log.Fatal(exception.Message);
        }
    }
}