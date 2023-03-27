# YooAssetsExtension
===

[![GitHub Actions](https://github.com/Cysharp/UniTask/workflows/Build-Debug/badge.svg)](https://github.com/Cysharp/UniTask/actions) [![Releases](https://img.shields.io/github/release/Cysharp/UniTask.svg)](https://github.com/Cysharp/UniTask/releases) [![Readme_CN](https://img.shields.io/badge/YooAssetsExtension-%E4%B8%AD%E6%96%87%E6%96%87%E6%A1%A3-red)](https://github.com/Cysharp/UniTask/blob/master/README_CN.md)

为提供GameFramework提供YooAssets的接入

# 文件说明

- YooAssetsComponent ---- YooAssets资源组件
- YooAssetsManager ----  YooAssets资源管理器

# 使用说明

GameModule 添加相应模块
```csharp 
/// <summary>
/// 获取配置组件。
/// </summary>
public static YooAssetsComponent YooAssets { get; private set; }

/// <summary>
/// 初始化系统框架模块。
/// </summary>
public static void InitFrameWorkComponents()
{
    YooAssets = Get<YooAssetsComponent>();
}
```

# 流程

### 1.Initialize
```csharp
private IEnumerator InitPackage()
{
    yield return new WaitForSeconds(1f);

    // 创建默认的资源包
    string packageName = "DefaultPackage";
    var package = YooAssets.TryGetPackage(packageName);
    if (package == null)
    {
        package = YooAssets.CreatePackage(packageName);
        YooAssets.SetDefaultPackage(package);
    }

    // 编辑器下的模拟模式
    InitializationOperation initializationOperation = null;
    if (playMode == EPlayMode.EditorSimulateMode)
    {
        var createParameters = new EditorSimulateModeParameters();
        createParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(packageName);
        initializationOperation = package.InitializeAsync(createParameters);
    }

    // 单机运行模式
    if (playMode == EPlayMode.OfflinePlayMode)
    {
        var createParameters = new OfflinePlayModeParameters();
        createParameters.DecryptionServices = new GameDecryptionServices();
        initializationOperation = package.InitializeAsync(createParameters);
    }

    // 联机运行模式
    if (playMode == EPlayMode.HostPlayMode)
    {
        var createParameters = new HostPlayModeParameters();
        createParameters.DecryptionServices = new GameDecryptionServices();
        createParameters.QueryServices = new GameQueryServices();
        createParameters.DefaultHostServer = GetHostServerURL();
        createParameters.FallbackHostServer = GetHostServerURL();
        initializationOperation = package.InitializeAsync(createParameters);
    }

    yield return initializationOperation;
    if (package.InitializeStatus == EOperationStatus.Succeed)
    {
        _machine.ChangeState<FsmUpdateVersion>();
    }
    else
    {
        Debug.LogWarning($"{initializationOperation.Error}");
    }
}
```
### 2.UpdateVersion
```csharp
private IEnumerator GetStaticVersion()
{
    yield return new WaitForSecondsRealtime(0.5f);

    var package = YooAssets.GetPackage("DefaultPackage");
    var operation = package.UpdatePackageVersionAsync();
    yield return operation;

    if (operation.Status == EOperationStatus.Succeed)
    {
        string packageVersion = operation.PackageVersion;
        _machine.ChangeState<FsmUpdateManifest>();
    }
    else
    {
        Debug.LogWarning(operation.Error);
    }
}
```
### 3.UpdateManifest
```csharp
private IEnumerator UpdateManifest()
{
    yield return new WaitForSecondsRealtime(0.5f);

    var package = YooAssets.GetPackage("DefaultPackage");
    var operation = package.UpdatePackageManifestAsync(string:packageVersion);
    yield return operation;

    if(operation.Status == EOperationStatus.Succeed)
    {
        _machine.ChangeState<FsmCreateDownloader>();
    }
    else
    {
        Debug.LogWarning(operation.Error);
    }
}
```
### 4.CreateDownloader
```csharp
IEnumerator CreateDownloader()
{
    yield return new WaitForSecondsRealtime(0.5f);

    int downloadingMaxNum = 10;
    int failedTryAgain = 3;
    var downloader = YooAssets.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
    ResourceDownloaderOperation Downloader = downloader;

    if (downloader.TotalDownloadCount == 0)
    {
        Debug.Log("Not found any download files !");
        _machine.ChangeState<FsmDownloadOver>();
    }
    else
    {
        //A total of 10 files were found that need to be downloaded
        Debug.Log($"Found total {downloader.TotalDownloadCount} files that need download ！");

        // 发现新更新文件后，挂起流程系统
        // 注意：开发者需要在下载前检测磁盘空间不足
        int totalDownloadCount = downloader.TotalDownloadCount;
        long totalDownloadBytes = downloader.TotalDownloadBytes;
        Event.SendEventMessage(totalDownloadCount, totalDownloadBytes);
    }
}
```
### 5.DownloadOver

### 6.ClearCache
```csharp
void OnClearCache()
{
    var package = YooAsset.YooAssets.GetPackage("DefaultPackage");
    var operation = package.ClearUnusedCacheFilesAsync();
    operation.Completed += Operation_Completed;
}
```