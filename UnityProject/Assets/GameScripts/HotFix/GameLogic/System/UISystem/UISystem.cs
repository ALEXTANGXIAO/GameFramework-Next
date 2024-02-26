using System;
using System.Collections.Generic;
using GameBase;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameLogic
{
    /// <summary>
    /// UI系统。
    /// </summary>
    public sealed partial class UISystem : BaseLogicSys<UISystem>
    {
        private bool m_EnableErrorLog = true;

        private readonly List<UIWindow> _stack = new List<UIWindow>(128);

        internal const int LAYER_DEEP = 2000;
        internal const int WINDOW_DEEP = 100;
        internal const int WINDOW_HIDE_LAYER = 2; // Ignore Raycast
        internal const int WINDOW_SHOW_LAYER = 5; // UI

        /// <summary>
        /// UI根节点。
        /// </summary>
        public Transform UICanvasTransform { private set; get; }

        /// <summary>
        /// UI根节点Canvas。
        /// </summary>
        public Canvas UICanvas { private set; get; }

        /// <summary>
        /// UI根节点。
        /// </summary>
        public Camera UICamera { private set; get; }

        private ErrorLogger _errorLogger;
        
        public override bool OnInit()
        {
            base.OnInit();
            
            Log.Info("OnInit UISystem");
            
            UICanvasTransform = GameObject.Find("UIRoot/UICanvas").transform;

            UICanvas = UICanvasTransform.GetComponent<Canvas>();

            UICamera = GameObject.Find("UIRoot/UICamera").GetComponent<Camera>();

            UICanvasTransform.gameObject.layer = LayerMask.NameToLayer("UI");

            switch (GameModule.Debugger.ActiveWindowType)
            {
                case DebuggerActiveWindowType.AlwaysOpen:
                    m_EnableErrorLog = true;
                    break;

                case DebuggerActiveWindowType.OnlyOpenWhenDevelopment:
                    m_EnableErrorLog = Debug.isDebugBuild;
                    break;

                case DebuggerActiveWindowType.OnlyOpenInEditor:
                    m_EnableErrorLog = Application.isEditor;
                    break;

                default:
                    m_EnableErrorLog = false;
                    break;
            }

            if (m_EnableErrorLog)
            {
                _errorLogger = new ErrorLogger();
            }

            UIController.RegisterAllController();
            
            return true;
        }

        public override void OnDestroy()
        {
            if (_errorLogger != null)
            {
                _errorLogger.Dispose();
                _errorLogger = null;
            }

            CloseAll();
        }

        /// <summary>
        /// 获取所有层级下顶部的窗口。
        /// </summary>
        public UIWindow GetTopWindow()
        {
            if (_stack.Count == 0)
            {
                return null;
            }

            UIWindow topWindow = _stack[^1];
            return topWindow;
        }

        /// <summary>
        /// 获取指定层级下顶部的窗口。
        /// </summary>
        public UIWindow GetTopWindow(int layer)
        {
            UIWindow lastOne = null;
            for (int i = 0; i < _stack.Count; i++)
            {
                if (_stack[i].WindowLayer == layer)
                {
                    lastOne = _stack[i];
                }
            }

            if (lastOne == null)
            {
                return null;
            }

            return lastOne;
        }
        
        /// <summary>
        /// 获取指定层级下顶部的窗口。
        /// </summary>
        public UIWindow GetTopWindow(UILayer layer)
        {
            return GetTopWindow((int)layer);
        }

        /// <summary>
        /// 是否有任意窗口正在加载。
        /// </summary>
        public bool IsAnyLoading()
        {
            for (int i = 0; i < _stack.Count; i++)
            {
                var window = _stack[i];
                if (window.IsLoadDone == false)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 查询窗口是否存在。
        /// </summary>
        /// <typeparam name="T">界面类型。</typeparam>
        /// <returns>是否存在。</returns>
        public bool HasWindow<T>()
        {
            return HasWindow(typeof(T));
        }

        /// <summary>
        /// 查询窗口是否存在。
        /// </summary>
        /// <param name="type">界面类型。</param>
        /// <returns>是否存在。</returns>
        public bool HasWindow(Type type)
        {
            return IsContains(type.FullName);
        }

        /// <summary>
        /// 异步打开窗口。
        /// </summary>
        /// <param name="userDatas">用户自定义数据。</param>
        /// <returns>打开窗口操作句柄。</returns>
        public void ShowUIAsync<T>(params System.Object[] userDatas) where T : UIWindow
        {
            ShowUIImp(typeof(T), true, userDatas);
        }

        /// <summary>
        /// 异步打开窗口。
        /// </summary>
        /// <param name="type">界面类型。</param>
        /// <param name="userDatas">用户自定义数据。</param>
        /// <returns>打开窗口操作句柄。</returns>
        public void ShowUIAsync(Type type, params System.Object[] userDatas)
        {
            ShowUIImp(type, true, userDatas);
        }

        /// <summary>
        /// 同步打开窗口。
        /// </summary>
        /// <typeparam name="T">窗口类。</typeparam>
        /// <param name="userDatas">用户自定义数据。</param>
        /// <returns>打开窗口操作句柄。</returns>
        public void ShowUI<T>(params System.Object[] userDatas) where T : UIWindow
        {
            ShowUIImp(typeof(T), false, userDatas);
        }

        /// <summary>
        /// 同步打开窗口。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userDatas"></param>
        /// <returns>打开窗口操作句柄。</returns>
        public void ShowUI(Type type, params System.Object[] userDatas)
        {
            ShowUIImp(type, false, userDatas);
        }

        private void ShowUIImp(Type type, bool isAsync, params System.Object[] userDatas)
        {
            string windowName = type.FullName;

            // 如果窗口已经存在
            if (IsContains(windowName))
            {
                UIWindow window = GetWindow(windowName);
                Pop(window); //弹出窗口
                Push(window); //重新压入
                window.TryInvoke(OnWindowPrepare, userDatas);
            }
            else
            {
                UIWindow window = CreateInstance(type);
                Push(window); //首次压入
                window.InternalLoad(window.AssetName, OnWindowPrepare, isAsync, userDatas).Forget();
            }
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public void CloseUI<T>() where T : UIWindow
        {
            CloseUI(typeof(T));
        }

        public void CloseUI(Type type)
        {
            string windowName = type.FullName;
            UIWindow window = GetWindow(windowName);
            if (window == null)
                return;

            window.InternalDestroy();
            Pop(window);
            OnSortWindowDepth(window.WindowLayer);
            OnSetWindowVisible();
        }
        
        public void HideUI<T>() where T : UIWindow
        {
            HideUI(typeof(T));
        }

        public void HideUI(Type type)
        {
            string windowName = type.FullName;
            UIWindow window = GetWindow(windowName);
            if (window == null)
            {
                return;
            }

            if (window.HideTimeToClose <= 0)
            {
                CloseUI(type);
                return;
            }
            
            window.Visible = false;
            window.HideTimerId = GameModule.Timer.AddOnceTimer(window.HideTimeToClose * 1000, () =>
            {
                CloseUI(type);
            });
        }

        /// <summary>
        /// 关闭所有窗口。
        /// </summary>
        public void CloseAll()
        {
            for (int i = 0; i < _stack.Count; i++)
            {
                UIWindow window = _stack[i];
                window.InternalDestroy();
            }

            _stack.Clear();
        }

        /// <summary>
        /// 关闭所有窗口除了。
        /// </summary>
        public void CloseAllWithOut(UIWindow withOut)
        {
            for (int i = _stack.Count - 1; i >= 0; i--)
            {
                UIWindow window = _stack[i];
                if (window == withOut)
                {
                    continue;
                }

                window.InternalDestroy();
                _stack.RemoveAt(i);
            }
        }

        /// <summary>
        /// 关闭所有窗口除了。
        /// </summary>
        public void CloseAllWithOut<T>() where T : UIWindow
        {
            for (int i = _stack.Count - 1; i >= 0; i--)
            {
                UIWindow window = _stack[i];
                if (window.GetType() == typeof(T))
                {
                    continue;
                }

                window.InternalDestroy();
                _stack.RemoveAt(i);
            }
        }

        private void OnWindowPrepare(UIWindow window)
        {
            OnSortWindowDepth(window.WindowLayer);
            window.InternalCreate();
            window.InternalRefresh();
            OnSetWindowVisible();
        }

        private void OnSortWindowDepth(int layer)
        {
            int depth = layer * LAYER_DEEP;
            for (int i = 0; i < _stack.Count; i++)
            {
                if (_stack[i].WindowLayer == layer)
                {
                    _stack[i].Depth = depth;
                    depth += WINDOW_DEEP;
                }
            }
        }

        private void OnSetWindowVisible()
        {
            bool isHideNext = false;
            for (int i = _stack.Count - 1; i >= 0; i--)
            {
                UIWindow window = _stack[i];
                if (isHideNext == false)
                {
                    window.Visible = true;
                    if (window.IsPrepare && window.FullScreen)
                    {
                        isHideNext = true;
                    }
                }
                else
                {
                    window.Visible = false;
                }
            }
        }

        private UIWindow CreateInstance(Type type)
        {
            UIWindow window = Activator.CreateInstance(type) as UIWindow;
            WindowAttribute attribute = Attribute.GetCustomAttribute(type, typeof(WindowAttribute)) as WindowAttribute;

            if (window == null)
                throw new GameFrameworkException($"Window {type.FullName} create instance failed.");

            if (attribute != null)
            {
                string assetName = string.IsNullOrEmpty(attribute.Location) ? type.Name : attribute.Location;
                window.Init(type.FullName, attribute.WindowLayer, attribute.FullScreen, assetName, attribute.FromResources, attribute.HideTimeToClose);
            }
            else
            {
                window.Init(type.FullName, (int)UILayer.UI, fullScreen: window.FullScreen, assetName: type.Name, fromResources: false, hideTimeToClose: 10);
            }

            return window;
        }

        private UIWindow GetWindow(string windowName)
        {
            for (int i = 0; i < _stack.Count; i++)
            {
                UIWindow window = _stack[i];
                if (window.WindowName == windowName)
                {
                    return window;
                }
            }

            return null;
        }

        private bool IsContains(string windowName)
        {
            for (int i = 0; i < _stack.Count; i++)
            {
                UIWindow window = _stack[i];
                if (window.WindowName == windowName)
                {
                    return true;
                }
            }

            return false;
        }

        private void Push(UIWindow window)
        {
            // 如果已经存在
            if (IsContains(window.WindowName))
                throw new GameFrameworkException($"Window {window.WindowName} is exist.");

            // 获取插入到所属层级的位置
            int insertIndex = -1;
            for (int i = 0; i < _stack.Count; i++)
            {
                if (window.WindowLayer == _stack[i].WindowLayer)
                {
                    insertIndex = i + 1;
                }
            }

            // 如果没有所属层级，找到相邻层级
            if (insertIndex == -1)
            {
                for (int i = 0; i < _stack.Count; i++)
                {
                    if (window.WindowLayer > _stack[i].WindowLayer)
                    {
                        insertIndex = i + 1;
                    }
                }
            }

            // 如果是空栈或没有找到插入位置
            if (insertIndex == -1)
            {
                insertIndex = 0;
            }

            // 最后插入到堆栈
            _stack.Insert(insertIndex, window);
        }

        private void Pop(UIWindow window)
        {
            // 从堆栈里移除
            _stack.Remove(window);
        }

        public override void OnUpdate()
        {
            if (_stack == null)
            {
                return;
            }

            int count = _stack.Count;
            for (int i = 0; i < _stack.Count; i++)
            {
                if (_stack.Count != count)
                {
                    break;
                }

                var window = _stack[i];
                window.InternalUpdate();
            }
        }
    }
}