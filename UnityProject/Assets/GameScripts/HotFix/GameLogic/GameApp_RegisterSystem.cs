using System.Collections.Generic;
using GameBase;
using GameFramework;
using UnityGameFramework.Runtime;

public partial class GameApp
{
    private List<ILogicSys> _listLogicMgr;

    private void InitSystem()
    {
        _listLogicMgr = new List<ILogicSys>();
        RegisterAllLogicSystem();
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
    }

    private void RegisterAllLogicSystem()
    {
        var targetType = typeof(ILogicSys);
        List<ILogicSys> result = new List<ILogicSys>();
        var allTypes = System.Reflection.Assembly.GetCallingAssembly().GetTypes();
        foreach (var type in allTypes)
        {
            if (type.IsAbstract)
            {
                continue;
            }

            System.Type[] tfs = type.GetInterfaces();
            foreach (var tf in tfs)
            {
                if (tf.FullName == targetType.FullName)
                {
                    ILogicSys a = System.Activator.CreateInstance(type) as ILogicSys;
                    result.Add(a);
                }
            }
        }

        foreach (var uiController in result)
        {
            AddLogicSys(uiController);
        }
    }

    /// <summary>
    /// 注册逻辑系统。
    /// </summary>
    /// <param name="logicSys">ILogicSys</param>
    /// <returns></returns>
    protected bool AddLogicSys(ILogicSys logicSys)
    {
        if (_listLogicMgr.Contains(logicSys))
        {
            Log.Fatal("Repeat add logic system: {0}", logicSys.GetType().Name);
            return false;
        }

        if (!logicSys.OnInit())
        {
            Log.Fatal("{0} Init failed", logicSys.GetType().Name);
            return false;
        }

        _listLogicMgr.Add(logicSys);

        return true;
    }
}