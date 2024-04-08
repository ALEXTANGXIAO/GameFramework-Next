using System;
using System.Collections.Generic;

namespace GameFramework
{
    /// <summary>
    /// 游戏框架系统管理。
    /// </summary>
    public static class GameFrameworkSystem
    {
        private const int DesignModuleCount = 16;
        
        private const string ModuleRootNameSpace = "GameFramework.";
        
        private static readonly Dictionary<Type, GameFrameworkModule> s_GameFrameworkModuleMaps = new Dictionary<Type, GameFrameworkModule>(DesignModuleCount);
        
        private static readonly GameFrameworkLinkedList<GameFrameworkModule> s_GameFrameworkModules = new GameFrameworkLinkedList<GameFrameworkModule>();

        private static readonly GameFrameworkLinkedList<GameFrameworkModule> s_UpdateModules = new GameFrameworkLinkedList<GameFrameworkModule>();
        
        private static int s_ExecuteCount = 0;
        
        private static readonly List<IUpdateModule> s_UpdateExecuteList = new List<IUpdateModule>(DesignModuleCount);

        private static bool s_IsExecuteListDirty;
        
        /// <summary>
        /// 所有游戏框架模块轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public static void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (s_IsExecuteListDirty)
            {
                s_IsExecuteListDirty = false;
                BuildExecuteList();
            }
            // 原版存在空遍历、foreach迭代器对stack存在开销，且链表在内存中的布局非连续，用数组连续布局的内存分布遍历更有利于CPU时钟。
            for (int i = 0; i < s_ExecuteCount; i++)
            {
                s_UpdateExecuteList[i].Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 关闭并清理所有游戏框架模块。
        /// </summary>
        public static void Shutdown()
        {
            for (LinkedListNode<GameFrameworkModule> current = s_GameFrameworkModules.Last; current != null; current = current.Previous)
            {
                current.Value.Shutdown();
            }

            s_GameFrameworkModuleMaps.Clear();
            s_GameFrameworkModules.Clear();
            s_UpdateModules.Clear();
            s_UpdateExecuteList.Clear();
            s_ExecuteCount = 0;
            s_IsExecuteListDirty = false;
            ReferencePool.ClearAll();
            Utility.Marshal.FreeCachedHGlobal();
            GameFrameworkLog.SetLogHelper(null);
        }

        /// <summary>
        /// 获取游戏框架模块。
        /// </summary>
        /// <typeparam name="T">要获取的游戏框架模块类型。</typeparam>
        /// <returns>要获取的游戏框架模块。</returns>
        /// <remarks>如果要获取的游戏框架模块不存在，则自动创建该游戏框架模块。</remarks>
        public static T GetModule<T>() where T : class
        {
            Type interfaceType = typeof(T);
            if (!interfaceType.IsInterface)
            {
                throw new GameFrameworkException(Utility.Text.Format("You must get module by interface, but '{0}' is not.", interfaceType.FullName));
            }

            if (!interfaceType.FullName.StartsWith(ModuleRootNameSpace, StringComparison.Ordinal))
            {
                throw new GameFrameworkException(Utility.Text.Format("You must get a Game Framework module, but '{0}' is not.", interfaceType.FullName));
            }

            string moduleName = Utility.Text.Format("{0}.{1}", interfaceType.Namespace, interfaceType.Name.Substring(1));
            Type moduleType = Type.GetType(moduleName);
            if (moduleType == null)
            {
                moduleName = Utility.Text.Format("{0}.{1}", interfaceType.Namespace, interfaceType.Name);
                moduleType = Type.GetType(moduleName);
                if (moduleType == null)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Can not find Game Framework module type '{0}'.", moduleName));
                }
            }

            return GetModule(moduleType) as T;
        }

        /// <summary>
        /// 获取游戏框架模块。
        /// </summary>
        /// <param name="moduleType">要获取的游戏框架模块类型。</param>
        /// <returns>要获取的游戏框架模块。</returns>
        /// <remarks>如果要获取的游戏框架模块不存在，则自动创建该游戏框架模块。</remarks>
        private static GameFrameworkModule GetModule(Type moduleType)
        {
            return s_GameFrameworkModuleMaps.TryGetValue(moduleType, out GameFrameworkModule module) ? module : CreateModule(moduleType);
        }

        /// <summary>
        /// 创建游戏框架模块。
        /// </summary>
        /// <param name="moduleType">要创建的游戏框架模块类型。</param>
        /// <returns>要创建的游戏框架模块。</returns>
        private static GameFrameworkModule CreateModule(Type moduleType)
        {
            GameFrameworkModule module = (GameFrameworkModule)Activator.CreateInstance(moduleType);
            if (module == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Can not create module '{0}'.", moduleType.FullName));
            }

            s_GameFrameworkModuleMaps[moduleType] = module;

            LinkedListNode<GameFrameworkModule> current = s_GameFrameworkModules.First;
            while (current != null)
            {
                if (module.Priority > current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            if (moduleType.GetInterface(nameof(IUpdateModule)) != null)
            {
                LinkedListNode<GameFrameworkModule> currentUpdate = s_UpdateModules.First;
                while (currentUpdate != null)
                {
                    if (module.Priority > currentUpdate.Value.Priority)
                    {
                        break;
                    }

                    currentUpdate = currentUpdate.Next;
                }

                if (currentUpdate != null)
                {
                    s_UpdateModules.AddBefore(currentUpdate, module);
                }
                else
                {
                    s_UpdateModules.AddLast(module);
                }

                s_IsExecuteListDirty = true;
            }

            return module;
        }

        /// <summary>
        /// 构造执行队列。
        /// </summary>
        private static void BuildExecuteList()
        {
            s_UpdateExecuteList.Clear();

            foreach (var module in s_UpdateModules)
            {
                if (module is IUpdateModule updateModule)
                {
                    s_UpdateExecuteList.Add(updateModule);
                }
            }

            s_ExecuteCount = s_UpdateExecuteList.Count;
        }
    }
}
