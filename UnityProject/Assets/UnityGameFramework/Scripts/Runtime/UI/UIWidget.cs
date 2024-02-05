using System;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public abstract class UIWidget:MonoBehaviour,IUIBehaviour
    {
        /// <summary>
        /// 获取界面实例。
        /// </summary>
        public GameObject Handle
        {
            get
            {
                return gameObject;
            }
        }
        
        /// <summary>
        /// 获取界面实例父节点。
        /// </summary>
        public Transform Parent
        {
            get
            {
                return gameObject.transform.parent;
            }
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
        /// OnCreate
        /// </summary>
        public virtual void OnCreate()
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
        /// 关闭。
        /// </summary>
        /// <param name="isShutdown">是否是关闭界面管理器时触发。</param>
        /// <param name="userData">用户自定义数据。</param>
        public virtual void OnClose(bool isShutdown, object userData)
        {
            
        }
        
        /// <summary>
        /// 界面暂停。
        /// </summary>
        public virtual void OnPause()
        {
            
        }

        /// <summary>
        /// 界面暂停恢复。
        /// </summary>
        public virtual void OnResume()
        {
            
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
        EventComponent m_EventComponent = GameSystem.GetComponent<EventComponent>();

        private List<int> m_listEventTypes;
        
        private List<EventHandler<GameEventArgs>> m_listHandles;
        
        public void AddUIEvent(int eventType, EventHandler<GameEventArgs> handler)
        {
            if (m_listEventTypes == null)
            {
                m_listEventTypes = new List<int>();
            }

            if (m_listHandles == null)
            {
                m_listHandles = new List<EventHandler<GameEventArgs>>();
            }
            
            m_listEventTypes.Add(eventType);
            
            m_listHandles.Add(handler);

            if (m_EventComponent != null)
            {
                m_EventComponent.Subscribe(eventType,handler);
            }
        }

        public void RemoveAllUIEvent()
        {
            
            if (m_listEventTypes == null)
            {
                return;
            }
            
            for (int i = 0; i < m_listEventTypes.Count; ++i)
            {
                var eventType = m_listEventTypes[i];
                
                var handler = m_listHandles[i];
                
                m_EventComponent.Unsubscribe(eventType, handler);
            }
            m_listEventTypes.Clear();
            
            m_listHandles.Clear();
        }
        #endregion
    }
}