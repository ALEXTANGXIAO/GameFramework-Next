using System;
using System.Collections.Generic;
using UGFExtensions.Texture;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 游戏模块
/// </summary>
public class GameModule : MonoBehaviour
{
    #region BaseComponents
    /// <summary>
    /// 获取游戏基础组件。
    /// </summary>
    public static BaseComponent Base { get; private set; }

    /// <summary>
    /// 获取调试组件。
    /// </summary>
    public static DebuggerComponent Debugger { get; private set; }

    /// <summary>
    /// 获取下载组件。
    /// </summary>
    public static DownloadComponent Download { get; private set; }

    /// <summary>
    /// 获取实体组件。
    /// </summary>
    public static EntityComponent Entity { get; private set; }

    /// <summary>
    /// 获取事件组件。
    /// </summary>
    public static EventComponent Event { get; private set; }

    /// <summary>
    /// 获取文件系统组件。
    /// </summary>
    public static FileSystemComponent FileSystem { get; private set; }

    /// <summary>
    /// 获取有限状态机组件。
    /// </summary>
    public static FsmComponent Fsm { get; private set; }

    /// <summary>
    /// 获取本地化组件。
    /// </summary>
    public static LocalizationComponent Localization { get; private set; }

    /// <summary>
    /// 获取网络组件。
    /// </summary>
    public static NetworkComponent Network { get; private set; }

    /// <summary>
    /// 获取对象池组件。
    /// </summary>
    public static ObjectPoolComponent ObjectPool { get; private set; }

    /// <summary>
    /// 获取流程组件。
    /// </summary>
    public static ProcedureComponent Procedure { get; private set; }

    /// <summary>
    /// 获取资源组件。
    /// </summary>
    public static ResourceComponent Resource { get; private set; }

    /// <summary>
    /// 获取场景组件。
    /// </summary>
    public static SceneComponent Scene { get; private set; }

    /// <summary>
    /// 获取配置组件。
    /// </summary>
    public static SettingComponent Setting { get; private set; }

    /// <summary>
    /// 获取声音组件。
    /// </summary>
    public static SoundComponent Sound { get; private set; }

    /// <summary>
    /// 获取界面组件。
    /// </summary>
    public static UIComponent UI { get; private set; }

    /// <summary>
    /// 获取网络组件。
    /// </summary>
    public static WebRequestComponent WebRequest { get; private set; }
    
    /// <summary>
    /// 获取时间组件。
    /// </summary>
    public static TimerComponent Timer { get; private set; }
    
    /// <summary>
    /// 获取设置Texture组件。
    /// </summary>
    public static TextureSetComponent TextureSet{ get; private set; }
    
    /// <summary>
    /// 资源组件拓展。
    /// </summary>
    public static ResourceExtComponent ResourceExt { get; private set; }
    #endregion

    /// <summary>
    /// 初始化系统框架模块
    /// </summary>
    public static void InitFrameWorkComponents()
    {
        Base = Get<BaseComponent>();
        Debugger = Get<DebuggerComponent>();
        Download = Get<DownloadComponent>();
        Entity = Get<EntityComponent>();
        Event = Get<EventComponent>();
        FileSystem = Get<FileSystemComponent>();
        Fsm = Get<FsmComponent>();
        Localization = Get<LocalizationComponent>();
        Network = Get<NetworkComponent>();
        ObjectPool = Get<ObjectPoolComponent>();
        Procedure = Get<ProcedureComponent>();
        Resource = Get<ResourceComponent>();
        Scene = Get<SceneComponent>();
        Setting = Get<SettingComponent>();
        Sound = Get<SoundComponent>();
        UI = Get<UIComponent>();
        WebRequest = Get<WebRequestComponent>();
        Timer = Get<TimerComponent>();
        TextureSet = Get<TextureSetComponent>();
        ResourceExt = Get<ResourceExtComponent>();
    }

    public static void InitCustomComponents()
    {
        
    }
  
    private static readonly Dictionary<Type, GameFrameworkComponent> s_Components = new Dictionary<Type, GameFrameworkComponent>();

    public static T Get<T>() where T : GameFrameworkComponent
    {
        Type type = typeof(T);
        
        if (s_Components.TryGetValue(type, out GameFrameworkComponent component))
        {
            return (T)component;
        }
        
        component = UnityGameFramework.Runtime.GameSystem.GetComponent<T>();
        
        Log.Assert(condition:component != null,$"{typeof(T)} is null");
        
        s_Components.Add(type,component);

        return (T)component;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        InitFrameWorkComponents();
        InitCustomComponents();
    }
    
    public static void QuitApplication()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.ExecuteMenuItem("Edit/Play");

#endif
        Application.Quit();
    }
}