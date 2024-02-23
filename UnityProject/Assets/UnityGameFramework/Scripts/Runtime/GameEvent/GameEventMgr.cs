using System;
using GameFramework;
using System.Collections.Generic;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 游戏事件管理器。
    /// </summary>
    public class GameEventMgr : IReference
    {
        private readonly List<int> m_ListEventTypes;
        private readonly List<Delegate> m_ListHandles;
        private readonly bool m_IsInit = false;

        /// <summary>
        /// 游戏事件管理器构造函数。
        /// </summary>
        public GameEventMgr()
        {
            if (m_IsInit)
            {
                return;
            }

            m_IsInit = true;
            m_ListEventTypes = new List<int>();
            m_ListHandles = new List<Delegate>();
        }

        /// <summary>
        /// 清理内存对象回收入池。
        /// </summary>
        public void Clear()
        {
            if (!m_IsInit)
            {
                return;
            }

            for (int i = 0; i < m_ListEventTypes.Count; ++i)
            {
                var eventType = m_ListEventTypes[i];
                var handle = m_ListHandles[i];
                GameEvent.RemoveEventListener(eventType, handle);
            }

            m_ListEventTypes.Clear();
            m_ListHandles.Clear();
        }

        private void AddEventImp(int eventType, Delegate handler)
        {
            m_ListEventTypes.Add(eventType);
            m_ListHandles.Add(handler);
        }

        #region AddEvent

        public void AddEvent(int eventType, Action handler)
        {
            if (GameEvent.AddEventListener(eventType, handler))
            {
                AddEventImp(eventType, handler);
            }
        }

        public void AddEvent<T>(int eventType, Action<T> handler)
        {
            if (GameEvent.AddEventListener(eventType, handler))
            {
                AddEventImp(eventType, handler);
            }
        }

        public void AddEvent<T1, T2>(int eventType, Action<T1, T2> handler)
        {
            if (GameEvent.AddEventListener(eventType, handler))
            {
                AddEventImp(eventType, handler);
            }
        }

        public void AddEvent<T1, T2, T3>(int eventType, Action<T1, T2, T3> handler)
        {
            if (GameEvent.AddEventListener(eventType, handler))
            {
                AddEventImp(eventType, handler);
            }
        }

        public void AddEvent<T1, T2, T3, T4>(int eventType, Action<T1, T2, T3, T4> handler)
        {
            if (GameEvent.AddEventListener(eventType, handler))
            {
                AddEventImp(eventType, handler);
            }
        }

        public void AddEvent<T1, T2, T3, T4, T5>(int eventType, Action<T1, T2, T3, T4, T5> handler)
        {
            if (GameEvent.AddEventListener(eventType, handler))
            {
                AddEventImp(eventType, handler);
            }
        }

        #endregion
    }
}