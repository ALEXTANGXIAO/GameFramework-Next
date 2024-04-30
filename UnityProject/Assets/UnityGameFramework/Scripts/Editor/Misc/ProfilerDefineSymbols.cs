using UnityEditor;

namespace UnityGameFramework.Editor
{
    public class ProfilerDefineSymbols
    {
        private const string EnableFirstProfiler = "FIRST_PROFILER";
        private const string EnableProFiler = "T_PROFILER";
        
        private static readonly string[] AllProfilerDefineSymbols = new string[]
        {
            EnableFirstProfiler,
            EnableProFiler,
        };
        
        /// <summary>
        /// 禁用所有日志脚本宏定义。
        /// </summary>
        [MenuItem("Game Framework/Profiler Define Symbols/Disable All Profiler", false, 30)]
        public static void DisableAllProfiler()
        {
            foreach (string aboveLogScriptingDefineSymbol in AllProfilerDefineSymbols)
            {
                ScriptingDefineSymbols.RemoveScriptingDefineSymbol(aboveLogScriptingDefineSymbol);
            }
        }

        /// <summary>
        /// 开启所有日志脚本宏定义。
        /// </summary>
        [MenuItem("Game Framework/Profiler Define Symbols/Enable All Profiler", false, 31)]
        public static void EnableAllProfiler()
        {
            DisableAllProfiler();
            foreach (string aboveLogScriptingDefineSymbol in AllProfilerDefineSymbols)
            {
                ScriptingDefineSymbols.AddScriptingDefineSymbol(aboveLogScriptingDefineSymbol);
            }
        }
    }
}