using System;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GameMain
{
    public static class ColorUtils
    {
        #region ColorStr

        public const string White = "FFFFFF";
        public const string Black = "000000";
        public const string Red = "FF0000";
        public const string Green = "00FF18";
        public const string Oringe = "FF9400";
        public const string Exception = "FF00BD";
        #endregion

        public static string ToColor(this string str, string color)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            return string.Format("<color=#{0}>{1}</color>", color, str);
        }
    }

    public class TLogger : Singleton<TLogger>
    {
        public static void LogAssert(bool condition, string logStr = "")
        {
            if (!condition)
            {
                if (string.IsNullOrEmpty(logStr))
                {
                    logStr = string.Format("{0}", "Assert Failed");
                }
                Instance.Log(LogLevel.ASSERT, logStr);
            }
        }

        public static void LogAssert(bool condition, string format, params System.Object[] args)
        {
            if (!condition)
            {
                string logStr = string.Format(format, args);
                Instance.Log(LogLevel.ASSERT, logStr);
            }
        }

        public static void LogInfo(string logStr)
        {
            Instance.Log(LogLevel.INFO, logStr);
        }

        public static void LogInfo(string format, params System.Object[] args)
        {
            string logStr = string.Format(format, args);
            Instance.Log(LogLevel.INFO, logStr);
        }

        public static void LogInfoSuccess(string logStr)
        {
            Instance.Log(LogLevel.Successd, logStr);
        }

        public static void LogInfoSuccess(string format, params System.Object[] args)
        {
            string logStr = string.Format(format, args);
            Instance.Log(LogLevel.Successd, logStr);
        }

        public static void LogWarning(string logStr)
        {
            Instance.Log(LogLevel.WARNING, logStr);
        }

        public static void LogWarning(string format, params System.Object[] args)
        {
            string logStr = string.Format(format, args);
            Instance.Log(LogLevel.WARNING, logStr);
        }

        public static void LogError(string logStr)
        {
            Instance.Log(LogLevel.ERROR, logStr);
        }

        public static void LogError(string format, params System.Object[] args)
        {
            string logStr = string.Format(format, args);
            Instance.Log(LogLevel.ERROR, logStr);
        }

        public static void LogException(string logStr)
        {
            Instance.Log(LogLevel.EXCEPTION, logStr);
        }

        public static void LogException(string format, params System.Object[] args)
        {
            string msg = string.Format(format, args);
            Instance.Log(LogLevel.EXCEPTION, msg);
        }

        private StringBuilder GetFormatString(LogLevel logLevel, string logString, bool bColor)
        {
            _stringBuilder.Clear();
            switch (logLevel)
            {
                case LogLevel.Successd:
                    if (UseCustomColor)
                    {
                        _stringBuilder.AppendFormat("<color=#0099bc><b>[TEngine] ► </b></color><color=#00FF18><b>[SUCCESSED] ► </b></color>[{0}] - <color=#{2}>{1}</color>",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString, ColorUtils.Green);
                    }
                    else
                    {
                        _stringBuilder.AppendFormat(
                            bColor ? "<color=#0099bc><b>[TEngine] ► </b></color><color=gray><b>[INFO] ► </b></color>[{0}] - <color=#00FF18>{1}</color>" : "<color=#0099bc><b>[TEngine] ► </b></color><color=#00FF18><b>[SUCCESSED] ► </b></color>[{0}] - {1}",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString);
                    }
                    break;
                case LogLevel.INFO:
                    if (UseCustomColor)
                    {
                        _stringBuilder.AppendFormat("<color=#0099bc><b>[TEngine] ► </b></color><color=gray><b>[INFO] ► </b></color>[{0}] - <color=#{2}>{1}</color>" , 
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString,ColorUtils.Black);
                    }
                    else
                    {
                        _stringBuilder.AppendFormat(
                            bColor ? "<color=#0099bc><b>[TEngine] ► </b></color><color=gray><b>[INFO] ► </b></color>[{0}] - <color=gray>{1}</color>" : "<color=#0099bc><b>[TEngine] ► </b></color><color=gray><b>[INFO] ► </b></color>[{0}] - {1}",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString);
                    }
                    break;
                case LogLevel.ASSERT:
                    if (UseCustomColor)
                    {
                        _stringBuilder.AppendFormat("<color=#0099bc><b>[TEngine] ► </b></color><color=#FF00BD><b>[ASSERT] ► </b></color>[{0}] - <color=#{2}>{1}</color>",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString,ColorUtils.Exception);
                    }
                    else
                    {
                        _stringBuilder.AppendFormat(
                            bColor ? "<color=#0099bc><b>[TEngine] ► </b></color><color=#FF00BD><b>[ASSERT] ► </b></color>[{0}] - <color=green>{1}</color>" : "<color=#0099bc><b>[TEngine] ► </b></color><color=#FF00BD><b>[ASSERT] ► </b></color>[{0}] - {1}",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString);
                    }
                    break;
                case LogLevel.WARNING:
                    if (UseCustomColor)
                    {
                        _stringBuilder.AppendFormat("<color=#0099bc><b>[TEngine] ► </b></color><color=#FF9400><b>[WARNING] ► </b></color>[{0}] - <color=#{2}>{1}</color>",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString, ColorUtils.Oringe);
                    }
                    else
                    {
                        _stringBuilder.AppendFormat(
                            bColor
                                ? "<color=#0099bc><b>[TEngine] ► </b></color><color=#FF9400><b>[WARNING] ► </b></color>[{0}] - <color=yellow>{1}</color>"
                                : "<color=#0099bc><b>[TEngine] ► </b></color><color=#FF9400><b>[WARNING] ► </b></color>[{0}] - {1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"),
                            logString);
                    }
                    break;
                case LogLevel.ERROR:
                    if (UseCustomColor)
                    {
                        _stringBuilder.AppendFormat("<color=red><b>[ERROR] ► </b></color><color=#FF9400><b>[WARNING] ► </b></color>[{0}] - <color=#{2}>{1}</color>",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString, ColorUtils.Red);
                    }
                    else
                    {
                        _stringBuilder.AppendFormat(
                            bColor ? "<color=#0099bc><b>[TEngine] ► </b></color><color=red><b>[ERROR] ► </b></color>[{0}] - <color=red>{1}</color>" : "<color=#0099bc><b>[TEngine] ► </b></color><color=red><b>[ERROR] ► </b></color>[{0}] - {1}",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString);
                    }
                    break;
                case LogLevel.EXCEPTION:
                    if (UseCustomColor)
                    {
                        _stringBuilder.AppendFormat("<color=red><b>[ERROR] ► </b></color><color=red><b>[EXCEPTION] ► </b></color>[{0}] - <color=#{2}>{1}</color>",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString, ColorUtils.Exception);
                    }
                    else
                    {
                        _stringBuilder.AppendFormat(
                            bColor
                                ? "<color=#0099bc><b>[TEngine] ► </b></color><color=red><b>[EXCEPTION] ► </b></color>[{0}] - <color=red>{1}</color>"
                                : "<color=#0099bc><b>[TEngine] ► </b></color><color=red><b>[EXCEPTION] ► </b></color>[{0}] - {1}",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString);
                    }
                    break;
            }

            return _stringBuilder;
        }

        private void Log(LogLevel type, string logString)
        {
            if (_outputType == OutputType.NONE)
            {
                return;
            }

            if (type < _filterLevel)
            {
                return;
            }

            StringBuilder infoBuilder = GetFormatString(type, logString, UseSystemColor);
            string logStr = infoBuilder.ToString();

            //获取C#堆栈,Warning以上级别日志才获取堆栈
            if (type == LogLevel.ERROR || type == LogLevel.WARNING || type == LogLevel.EXCEPTION)
            {
                StackFrame[] stackFrames = new StackTrace().GetFrames();
                for (int i = 0; i < stackFrames.Length; i++)
                {
                    StackFrame frame = stackFrames[i];
                    string declaringTypeName = frame.GetMethod().DeclaringType.FullName;
                    string methodName = stackFrames[i].GetMethod().Name;

                    infoBuilder.AppendFormat("[{0}::{1}\n", declaringTypeName, methodName);
                }
            }

            if (type == LogLevel.INFO || type == LogLevel.Successd)
            {
                Debug.Log(logStr);
            }
            else if (type == LogLevel.WARNING)
            {
                Debug.LogWarning(logStr);
            }
            else if (type == LogLevel.ASSERT)
            {
                Debug.LogAssertion(logStr);
            }
            else if (type == LogLevel.ERROR)
            {
                Debug.LogError(logStr);
            }
            else if (type == LogLevel.EXCEPTION)
            {
                Debug.LogError(logStr);
            }
        }

        #region Properties
        public bool UseCustomColor = false;
        public bool UseSystemColor = true;

        public enum LogLevel
        {
            INFO,
            Successd,
            ASSERT,
            WARNING,
            ERROR,
            EXCEPTION,
        }

        [System.Flags]
        public enum OutputType
        {
            NONE = 0,
            EDITOR = 0x1,
            GUI = 0x2,
            FILE = 0x4
        }

        private LogLevel _filterLevel = LogLevel.INFO;
        private OutputType _outputType = OutputType.EDITOR;
        private StringBuilder _stringBuilder = new StringBuilder();
        #endregion
    }
}