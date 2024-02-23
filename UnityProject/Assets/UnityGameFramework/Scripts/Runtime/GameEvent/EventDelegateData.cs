using System;
using System.Collections.Generic;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 游戏事件数据类。
    /// </summary>
    internal class EventDelegateData
    {
        private readonly int m_EventType = 0;
        private readonly List<Delegate> m_ListExist = new List<Delegate>();
        private readonly List<Delegate> m_AddList = new List<Delegate>();
        private readonly List<Delegate> m_DeleteList = new List<Delegate>();
        private bool m_IsExecute = false;
        private bool m_IsDirty = false;

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        internal EventDelegateData(int eventType)
        {
            m_EventType = eventType;
        }

        /// <summary>
        /// 添加注册委托。
        /// </summary>
        /// <param name="handler">事件处理回调。</param>
        /// <returns>是否添加回调成功。</returns>
        internal bool AddHandler(Delegate handler)
        {
            if (m_ListExist.Contains(handler))
            {
                Log.Fatal("Repeated Add Handler");
                return false;
            }

            if (m_IsExecute)
            {
                m_IsDirty = true;
                m_AddList.Add(handler);
            }
            else
            {
                m_ListExist.Add(handler);
            }

            return true;
        }

        /// <summary>
        /// 移除反注册委托。
        /// </summary>
        /// <param name="handler">事件处理回调。</param>
        internal void RmvHandler(Delegate handler)
        {
            if (m_IsExecute)
            {
                m_IsDirty = true;
                m_DeleteList.Add(handler);
            }
            else
            {
                if (!m_ListExist.Remove(handler))
                {
                    Log.Fatal("Delete handle failed, not exist, EventId: {0}", StringId.HashToString(m_EventType));
                }
            }
        }

        /// <summary>
        /// 检测脏数据修正。
        /// </summary>
        private void CheckModify()
        {
            m_IsExecute = false;
            if (m_IsDirty)
            {
                foreach (var t in m_AddList)
                {
                    m_ListExist.Add(t);
                }

                m_AddList.Clear();

                foreach (var t in m_DeleteList)
                {
                    m_ListExist.Remove(t);
                }

                m_DeleteList.Clear();
            }
        }

        /// <summary>
        /// 回调调用。
        /// </summary>
        public void Callback()
        {
            m_IsExecute = true;
            foreach (var d in m_ListExist)
            {
                if (d is Action action)
                {
                    action();
                }
            }

            CheckModify();
        }

        /// <summary>
        /// 回调调用。
        /// </summary>
        /// <param name="arg1">事件参数1。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        public void Callback<TArg1>(TArg1 arg1)
        {
            m_IsExecute = true;
            foreach (var d in m_ListExist)
            {
                if (d is Action<TArg1> action)
                {
                    action(arg1);
                }
            }

            CheckModify();
        }

        /// <summary>
        /// 回调调用。
        /// </summary>
        /// <param name="arg1">事件参数1。</param>
        /// <param name="arg2">事件参数2。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        public void Callback<TArg1, TArg2>(TArg1 arg1, TArg2 arg2)
        {
            m_IsExecute = true;
            foreach (var d in m_ListExist)
            {
                if (d is Action<TArg1, TArg2> action)
                {
                    action(arg1, arg2);
                }
            }

            CheckModify();
        }

        /// <summary>
        /// 回调调用。
        /// </summary>
        /// <param name="arg1">事件参数1。</param>
        /// <param name="arg2">事件参数2。</param>
        /// <param name="arg3">事件参数3。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <typeparam name="TArg3">事件参数3类型。</typeparam>
        public void Callback<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            m_IsExecute = true;
            foreach (var d in m_ListExist)
            {
                if (d is Action<TArg1, TArg2, TArg3> action)
                {
                    action(arg1, arg2, arg3);
                }
            }

            CheckModify();
        }

        /// <summary>
        /// 回调调用。
        /// </summary>
        /// <param name="arg1">事件参数1。</param>
        /// <param name="arg2">事件参数2。</param>
        /// <param name="arg3">事件参数3。</param>
        /// <param name="arg4">事件参数4。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <typeparam name="TArg3">事件参数3类型。</typeparam>
        /// <typeparam name="TArg4">事件参数4类型。</typeparam>
        public void Callback<TArg1, TArg2, TArg3, TArg4>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
        {
            m_IsExecute = true;
            foreach (var d in m_ListExist)
            {
                if (d is Action<TArg1, TArg2, TArg3, TArg4> action)
                {
                    action(arg1, arg2, arg3, arg4);
                }
            }

            CheckModify();
        }

        /// <summary>
        /// 回调调用。
        /// </summary>
        /// <param name="arg1">事件参数1。</param>
        /// <param name="arg2">事件参数2。</param>
        /// <param name="arg3">事件参数3。</param>
        /// <param name="arg4">事件参数4。</param>
        /// <param name="arg5">事件参数5。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <typeparam name="TArg3">事件参数3类型。</typeparam>
        /// <typeparam name="TArg4">事件参数4类型。</typeparam>
        /// <typeparam name="TArg5">事件参数5类型。</typeparam>
        public void Callback<TArg1, TArg2, TArg3, TArg4, TArg5>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5)
        {
            m_IsExecute = true;
            foreach (var d in m_ListExist)
            {
                if (d is Action<TArg1, TArg2, TArg3, TArg4, TArg5> action)
                {
                    action(arg1, arg2, arg3, arg4, arg5);
                }
            }

            CheckModify();
        }
        
        /// <summary>
        /// 回调调用。
        /// </summary>
        /// <param name="arg1">事件参数1。</param>
        /// <param name="arg2">事件参数2。</param>
        /// <param name="arg3">事件参数3。</param>
        /// <param name="arg4">事件参数4。</param>
        /// <param name="arg5">事件参数5。</param>
        /// <param name="arg6">事件参数6。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <typeparam name="TArg3">事件参数3类型。</typeparam>
        /// <typeparam name="TArg4">事件参数4类型。</typeparam>
        /// <typeparam name="TArg5">事件参数5类型。</typeparam>
        /// <typeparam name="TArg6">事件参数6类型。</typeparam>
        public void Callback<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6)
        {
            m_IsExecute = true;
            foreach (var d in m_ListExist)
            {
                if (d is Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> action)
                {
                    action(arg1, arg2, arg3, arg4, arg5, arg6);
                }
            }

            CheckModify();
        }
    }
}