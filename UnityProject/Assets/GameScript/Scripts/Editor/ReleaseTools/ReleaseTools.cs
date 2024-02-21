using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using YooAsset.Editor;
using BuildResult = UnityEditor.Build.Reporting.BuildResult;

namespace UnityGameFramework.Editor
{
    /// <summary>
    /// 打包工具类。
    /// <remarks>通过CommandLineReader可以不前台开启Unity实现静默打包以及CLI工作流，详见CommandLineReader.cs example1</remarks>
    /// </summary>
    public static class ReleaseTools
    {
        public static void BuildDll()
        {
            string platform = CommandLineReader.GetCustomArgument("platform");
            if (string.IsNullOrEmpty(platform))
            {
                Debug.LogError($"Build Asset Bundle Error！platform is null");
                return;
            }

            BuildTarget target = GetBuildTarget(platform);
            
            BuildDLLCommand.BuildAndCopyDlls(target);
        }

        public static void BuildAssetBundle()
        {
            string outputRoot = CommandLineReader.GetCustomArgument("outputRoot");
            if (string.IsNullOrEmpty(outputRoot))
            {
                Debug.LogError($"Build Asset Bundle Error！outputRoot is null");
                return;
            }

            string packageVersion = CommandLineReader.GetCustomArgument("packageVersion");
            if (string.IsNullOrEmpty(packageVersion))
            {
                Debug.LogError($"Build Asset Bundle Error！packageVersion is null");
                return;
            }

            string platform = CommandLineReader.GetCustomArgument("platform");
            if (string.IsNullOrEmpty(platform))
            {
                Debug.LogError($"Build Asset Bundle Error！platform is null");
                return;
            }

            BuildTarget target = GetBuildTarget(platform);
            BuildInternal(target, outputRoot);
            Debug.LogWarning($"Start BuildPackage BuildTarget:{target} outputPath:{outputRoot}");
        }

        private static BuildTarget GetBuildTarget(string platform)
        {
            BuildTarget target = BuildTarget.NoTarget;
            switch (platform)
            {
                case "Android":
                    target = BuildTarget.Android;
                    break;
                case "IOS":
                    target = BuildTarget.iOS;
                    break;
                case "Windows":
                    target = BuildTarget.StandaloneWindows64;
                    break;
                case "MacOS":
                    target = BuildTarget.StandaloneOSX;
                    break;
                case "Linux":
                    target = BuildTarget.StandaloneLinux64;
                    break;
                case "WebGL":
                    target = BuildTarget.WebGL;
                    break;
                case "Switch":
                    target = BuildTarget.Switch;
                    break;
                case "PS4":
                    target = BuildTarget.PS4;
                    break;
                case "PS5":
                    target = BuildTarget.PS5;
                    break;
            }

            return target;
        }

        private static void BuildInternal(BuildTarget buildTarget, string outputRoot, string packageVersion = "1.0")
        {
            Debug.Log($"开始构建 : {buildTarget}");

            var buildoutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot();
            var streamingAssetsRoot = AssetBundleBuilderHelper.GetStreamingAssetsRoot();
            // 构建参数
            BuiltinBuildParameters buildParameters = new BuiltinBuildParameters();
            buildParameters.BuildOutputRoot = buildoutputRoot;
            buildParameters.BuildinFileRoot = streamingAssetsRoot;
            buildParameters.BuildPipeline = EBuildPipeline.BuiltinBuildPipeline.ToString();
            buildParameters.BuildTarget = buildTarget;
            buildParameters.BuildMode = EBuildMode.ForceRebuild;
            buildParameters.PackageName = "DefaultPackage";
            buildParameters.PackageVersion = packageVersion;
            buildParameters.VerifyBuildingResult = true;
            buildParameters.FileNameStyle = EFileNameStyle.HashName;
            buildParameters.BuildinFileCopyOption = EBuildinFileCopyOption.None;
            buildParameters.BuildinFileCopyParams = string.Empty;
            buildParameters.EncryptionServices = new EncryptionNone();
            buildParameters.CompressOption = ECompressOption.LZ4;

            // 执行构建
            BuiltinBuildPipeline pipeline = new BuiltinBuildPipeline();
            var buildResult = pipeline.Run(buildParameters, true);
            if (buildResult.Success)
            {
                Debug.Log($"构建成功 : {buildResult.OutputPackageDirectory}");
            }
            else
            {
                Debug.LogError($"构建失败 : {buildResult.ErrorInfo}");
            }
        }

        [MenuItem("Game Framework/Quick Build/一键打包Window", false, 90)]
        public static void AutomationBuild()
        {
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            // BuildDLLCommand.BuildAndCopyDlls(target);
            AssetDatabase.Refresh();
            BuildInternal(target, Application.dataPath + "/../Builds/Windows", packageVersion: GetBuildPackageVersion());
            AssetDatabase.Refresh();
            BuildImp(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64, $"{Application.dataPath}/../Builds/Windows/Release_Windows.exe");
        }

        // 构建版本相关
        private static string GetBuildPackageVersion()
        {
            int totalMinutes = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
            return DateTime.Now.ToString("yyyy-MM-dd") + "-" + totalMinutes;
        }

        [MenuItem("Game Framework/Quick Build/一键打包Android", false, 90)]
        public static void AutomationBuildAndroid()
        {
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            // BuildDLLCommand.BuildAndCopyDlls(target);
            AssetDatabase.Refresh();
            BuildInternal(target, outputRoot: Application.dataPath + "/../Bundles", packageVersion: GetBuildPackageVersion());
            AssetDatabase.Refresh();
            BuildImp(BuildTargetGroup.Android, BuildTarget.Android, $"{Application.dataPath}/../Build/Android/{GetBuildPackageVersion()}Android.apk");
            // BuildImp(BuildTargetGroup.Android, BuildTarget.Android, $"{Application.dataPath}/../Build/Android/Android.apk");
        }

        [MenuItem("Game Framework/Quick Build/一键打包IOS", false, 90)]
        public static void AutomationBuildIOS()
        {
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            // BuildDLLCommand.BuildAndCopyDlls(target);
            AssetDatabase.Refresh();
            BuildInternal(target, outputRoot: Application.dataPath + "/../Bundles", packageVersion: GetBuildPackageVersion());
            AssetDatabase.Refresh();
            BuildImp(BuildTargetGroup.iOS, BuildTarget.iOS, $"{Application.dataPath}/../Build/IOS/XCode_Project");
        }

        public static void BuildImp(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, string locationPathName)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, BuildTarget.StandaloneWindows64);
            AssetDatabase.Refresh();

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new[] { "Assets/Scenes/main.unity" },
                locationPathName = locationPathName,
                targetGroup = buildTargetGroup,
                target = buildTarget,
                options = BuildOptions.None
            };
            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;
            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Build success: {summary.totalSize / 1024 / 1024} MB");
            }
            else
            {
                Debug.Log($"Build Failed" + summary.result);
            }
        }
    }
}