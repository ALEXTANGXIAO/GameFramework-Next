using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 界面逻辑基类。
    /// </summary>
    public abstract partial class UIFormLogic : MonoBehaviour, IUIBehaviour
    {
        private bool m_Available = false;
        private bool m_Visible = false;
        private UIForm m_UIForm = null;
        private Transform m_CachedTransform = null;
        private int m_OriginalLayer = 0;

        /// <summary>
        /// 获取界面。
        /// </summary>
        public UIForm UIForm
        {
            get { return m_UIForm; }
        }

        /// <summary>
        /// 获取或设置界面名称。
        /// </summary>
        public string Name
        {
            get { return gameObject.name; }
            set { gameObject.name = value; }
        }

        /// <summary>
        /// 获取界面是否可用。
        /// </summary>
        public bool Available
        {
            get { return m_Available; }
        }

        /// <summary>
        /// 获取或设置界面是否可见。
        /// </summary>
        public bool Visible
        {
            get { return m_Available && m_Visible; }
            set
            {
                if (!m_Available)
                {
                    Log.Warning("UI form '{0}' is not available.", Name);
                    return;
                }

                if (m_Visible == value)
                {
                    return;
                }

                m_Visible = value;
                InternalSetVisible(value);
            }
        }

        /// <summary>
        /// 获取已缓存的 Transform。
        /// </summary>
        public Transform CachedTransform
        {
            get { return m_CachedTransform; }
        }

        public List<UIWidget> UIWidgets = new List<UIWidget>();

        /// <summary>
        /// 界面初始化。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal virtual void OnInit(object userData)
        {
            if (m_CachedTransform == null)
            {
                m_CachedTransform = transform;
            }

            m_UIForm = GetComponent<UIForm>();
            m_OriginalLayer = gameObject.layer;
        }

        /// <summary>
        /// 界面回收。
        /// </summary>
        protected internal virtual void OnRecycle()
        {
        }

        /// <summary>
        /// ScriptGenerator
        /// </summary>
        public virtual void ScriptGenerator()
        {
        }

        /// <summary>
        /// 注册UI事件
        /// </summary>
        public virtual void RegisterEvent()
        {
        }

        /// <summary>
        /// OnCreate事件
        /// </summary>
        public virtual void OnCreate()
        {
        }

        /// <summary>
        /// 界面打开。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal virtual void OnOpen(object userData)
        {
            m_Available = true;
            Visible = true;
        }

        /// <summary>
        /// 界面关闭。
        /// </summary>
        /// <param name="isShutdown">是否是关闭界面管理器时触发。</param>
        /// <param name="userData">用户自定义数据。</param>
        public virtual void OnClose(bool isShutdown, object userData)
        {
            gameObject.SetLayerRecursively(m_OriginalLayer);
            Visible = false;
            m_Available = false;
        }

        /// <summary>
        /// 界面暂停。
        /// </summary>
        public virtual void OnPause()
        {
            Visible = false;
        }

        /// <summary>
        /// 界面暂停恢复。
        /// </summary>
        public virtual void OnResume()
        {
            Visible = true;
        }

        /// <summary>
        /// 界面遮挡。
        /// </summary>
        protected internal virtual void OnCover()
        {
        }

        /// <summary>
        /// 界面遮挡恢复。
        /// </summary>
        protected internal virtual void OnReveal()
        {
        }

        /// <summary>
        /// 界面激活。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal virtual void OnRefocus(object userData)
        {
        }

        /// <summary>
        /// 界面轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 界面深度改变。
        /// </summary>
        /// <param name="uiGroupDepth">界面组深度。</param>
        /// <param name="depthInUIGroup">界面在界面组中的深度。</param>
        protected internal virtual void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
        }

        /// <summary>
        /// 设置界面的可见性。
        /// </summary>
        /// <param name="visible">界面的可见性。</param>
        protected virtual void InternalSetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        protected virtual void Close()
        {
            GameSystem.GetComponent<UIComponent>().CloseUIForm(this.UIForm);
        }
        #region FindChildComponent

        public Transform FindChild(string path)
        {
            return DUnityUtil.FindChild(transform, path);
        }

        public Transform FindChild(Transform _transform, string path)
        {
            return DUnityUtil.FindChild(_transform, path);
        }

        public T FindChildComponent<T>(string path) where T : Component
        {
            return DUnityUtil.FindChildComponent<T>(transform, path);
        }

        public T FindChildComponent<T>(Transform _transform, string path) where T : Component
        {
            return DUnityUtil.FindChildComponent<T>(_transform, path);
        }

        #endregion

        #region UIEvent

        private GameEventMgr m_EventMgr;

        protected GameEventMgr EventMgr
        {
            get
            {
                if (m_EventMgr == null)
                {
                    m_EventMgr = GameFramework.ReferencePool.Acquire<GameEventMgr>();
                }

                return m_EventMgr;
            }
        }

        public void AddUIEvent(int eventType, Action handler)
        {
            EventMgr.AddEvent(eventType, handler);
        }

        protected void AddUIEvent<T>(int eventType, Action<T> handler)
        {
            EventMgr.AddEvent(eventType, handler);
        }

        protected void AddUIEvent<T, U>(int eventType, Action<T, U> handler)
        {
            EventMgr.AddEvent(eventType, handler);
        }

        protected void AddUIEvent<T, U, V>(int eventType, Action<T, U, V> handler)
        {
            EventMgr.AddEvent(eventType, handler);
        }

        protected void AddUIEvent<T, U, V, W>(int eventType, Action<T, U, V, W> handler)
        {
            EventMgr.AddEvent(eventType, handler);
        }

        public void RemoveAllUIEvent()
        {
            if (m_EventMgr != null)
            {
                GameFramework.ReferencePool.Release(m_EventMgr);
            }
        }

        #endregion

        #region UIWidget

        public T CreateWidgetByPath<T>(Transform parent, string assetPath) where T : UIWidget, new()
        {
            return null;
        }

        public T CreateWidgetByPrefab<T>(GameObject widgetObj, Transform parent = null) where T : UIWidget, new()
        {
            if (widgetObj != null)
            {
                UIWidget widget = null;

                if (parent == null)
                {
                    widget = widgetObj.AddComponent<T>();
                }
                else
                {
                    GameObject widgetRoot = UnityEngine.Object.Instantiate(widgetObj, parent);

                    widget = widgetRoot.AddComponent<T>();
                }

                CreateImp(widget);

                UIWidgets.Add(widget);

                return widget as T;
            }

            return null;
        }

        private void CreateImp(UIWidget uiWidget)
        {
            if (uiWidget == null)
            {
                return;
            }

            uiWidget.ScriptGenerator();

            uiWidget.RegisterEvent();

            uiWidget.OnCreate();
        }

        /// <summary>
        /// 调整图标数量
        /// </summary>
        public void AdjustIconNum<T>(List<T> listIcon, int tarNum, Transform parent, GameObject prefab = null,
            string assetPath = "")
            where T : UIWidget, new()
        {
            if (listIcon == null)
                listIcon = new List<T>();
            if (listIcon.Count < tarNum)
            {
                T tmpT = null;
                int needNum = tarNum - listIcon.Count;
                for (int iconIdx = 0; iconIdx < needNum; iconIdx++)
                {
                    if (prefab == null)
                    {
                        tmpT = CreateWidgetByPath<T>(parent, assetPath);
                    }
                    else
                    {
                        tmpT = CreateWidgetByPrefab<T>(prefab, parent);
                    }

                    listIcon.Add(tmpT);
                }
            }
            else if (listIcon.Count > tarNum)
            {
                RemoveUnUseItem<T>(listIcon, tarNum);
            }
        }

        public void AsyncAdjustIconNum<T>(List<T> listIcon, int tarNum, Transform parent, GameObject prefab = null,
            string assetPath = "", int maxNumPerFrame = 5,
            Action<T, int> updateAction = null) where T : UIWidget, new()
        {
            StartCoroutine(AsyncAdjustIconNumIE(listIcon, tarNum, parent, maxNumPerFrame, updateAction, prefab,
                assetPath));
        }

        /// <summary>
        /// 异步创建接口
        /// </summary>
        public IEnumerator AsyncAdjustIconNumIE<T>(List<T> listIcon, int tarNum, Transform parent, int maxNumPerFrame,
            Action<T, int> updateAction, GameObject prefab, string assetPath) where T : UIWidget, new()
        {
            if (listIcon == null)
            {
                listIcon = new List<T>();
            }

            int createCnt = 0;

            for (int i = 0; i < tarNum; i++)
            {
                T tmpT = null;
                if (i < listIcon.Count)
                {
                    tmpT = listIcon[i];
                }
                else
                {
                    if (prefab == null)
                    {
                        tmpT = CreateWidgetByPath<T>(parent, assetPath);
                    }
                    else
                    {
                        tmpT = CreateWidgetByPrefab<T>(prefab, parent);
                    }

                    listIcon.Add(tmpT);
                }

                int index = i;
                if (updateAction != null)
                {
                    updateAction(tmpT, index);
                }

                createCnt++;
                if (createCnt >= maxNumPerFrame)
                {
                    createCnt = 0;
                    yield return null;
                }
            }

            if (listIcon.Count > tarNum)
            {
                RemoveUnUseItem(listIcon, tarNum);
            }
        }

        private void RemoveUnUseItem<T>(List<T> listIcon, int tarNum) where T : UIWidget, new()
        {
            var removeIcon = new List<T>();
            for (int iconIdx = 0; iconIdx < listIcon.Count; iconIdx++)
            {
                var icon = listIcon[iconIdx];
                if (iconIdx >= tarNum)
                {
                    removeIcon.Add(icon);
                }
            }

            for (var index = 0; index < removeIcon.Count; index++)
            {
                var icon = removeIcon[index];
                listIcon.Remove(icon);
                icon.OnClose(false, null);
                Destroy(icon.gameObject);
            }
        }

        #endregion
    }
}