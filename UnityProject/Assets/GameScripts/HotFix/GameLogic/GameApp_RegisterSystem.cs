using System.Collections.Generic;
using GameBase;
using GameLogic;
using GameFramework;
using UnityGameFramework.Runtime;

public partial class GameApp
{
    private List<ILogicSys> m_ListLogicMgr;

    private void InitSystem()
    {
        m_ListLogicMgr = new List<ILogicSys>();
        CodeTypes.Instance.Init(s_HotfixAssembly.ToArray());
        EventInterfaceHelper.Init();
        RegisterAllSystem();
        InitSystemSetting();
    }

    /// <summary>
    /// 设置一些通用的系统属性。
    /// </summary>
    private void InitSystemSetting()
    {
    }

    /// <summary>
    /// 注册所有逻辑系统
    /// </summary>
    private void RegisterAllSystem()
    {
        AddLogicSys(UISystem.Instance);
    }

    /// <summary>
    /// 注册逻辑系统。
    /// </summary>
    /// <param name="logicSys">ILogicSys</param>
    /// <returns></returns>
    public bool AddLogicSys(ILogicSys logicSys)
    {
        if (m_ListLogicMgr.Contains(logicSys))
        {
            Log.Fatal("Repeat add logic system: {0}", logicSys.GetType().Name);
            return false;
        }

        if (!logicSys.OnInit())
        {
            Log.Fatal("{0} Init failed", logicSys.GetType().Name);
            return false;
        }

        m_ListLogicMgr.Add(logicSys);

        return true;
    }
}