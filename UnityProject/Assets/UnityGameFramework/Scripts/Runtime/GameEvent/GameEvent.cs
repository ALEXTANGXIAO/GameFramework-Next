using System;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 游戏全局事件类。
    /// </summary>
    public static class GameEvent
    {
        /// <summary>
        /// 全局事件管理器。
        /// </summary>
        private static readonly EventMgr s_EventMgr = new EventMgr();

        public static EventMgr EventMgr => s_EventMgr;

        #region 细分的注册接口

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件Handler。</param>
        /// <returns>是否监听成功。</returns>
        public static bool AddEventListener(int eventType, Action handler)
        {
            return s_EventMgr.Dispatcher.AddEventListener(eventType, handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<TArg1>(int eventType, Action<TArg1> handler)
        {
            return s_EventMgr.Dispatcher.AddEventListener(eventType, handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<TArg1, TArg2>(int eventType, Action<TArg1, TArg2> handler)
        {
            return s_EventMgr.Dispatcher.AddEventListener(eventType, handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <typeparam name="TArg3">事件参数3类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<TArg1, TArg2, TArg3>(int eventType, Action<TArg1, TArg2, TArg3> handler)
        {
            return s_EventMgr.Dispatcher.AddEventListener(eventType, handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <typeparam name="TArg3">事件参数3类型。</typeparam>
        /// <typeparam name="TArg4">事件参数4类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<TArg1, TArg2, TArg3, TArg4>(int eventType, Action<TArg1, TArg2, TArg3, TArg4> handler)
        {
            return s_EventMgr.Dispatcher.AddEventListener(eventType, handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <typeparam name="TArg3">事件参数3类型。</typeparam>
        /// <typeparam name="TArg4">事件参数4类型。</typeparam>
        /// <typeparam name="TArg5">事件参数5类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<TArg1, TArg2, TArg3, TArg4, TArg5>(int eventType, Action<TArg1, TArg2, TArg3, TArg4, TArg5> handler)
        {
            return s_EventMgr.Dispatcher.AddEventListener(eventType, handler);
        }
        
        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <typeparam name="TArg3">事件参数3类型。</typeparam>
        /// <typeparam name="TArg4">事件参数4类型。</typeparam>
        /// <typeparam name="TArg5">事件参数5类型。</typeparam>
        /// <typeparam name="TArg6">事件参数6类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(int eventType, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> handler)
        {
            return s_EventMgr.Dispatcher.AddEventListener(eventType, handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        public static void RemoveEventListener(int eventType, Action handler)
        {
            s_EventMgr.Dispatcher.RemoveEventListener(eventType, handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        public static void RemoveEventListener<TArg1>(int eventType, Action<TArg1> handler)
        {
            s_EventMgr.Dispatcher.RemoveEventListener(eventType, handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        public static void RemoveEventListener<TArg1, TArg2>(int eventType, Action<TArg1, TArg2> handler)
        {
            s_EventMgr.Dispatcher.RemoveEventListener(eventType, handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <typeparam name="TArg3">事件参数3类型。</typeparam>
        public static void RemoveEventListener<TArg1, TArg2, TArg3>(int eventType, Action<TArg1, TArg2, TArg3> handler)
        {
            s_EventMgr.Dispatcher.RemoveEventListener(eventType, handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <typeparam name="TArg3">事件参数3类型。</typeparam>
        /// <typeparam name="TArg4">事件参数4类型。</typeparam>
        public static void RemoveEventListener<TArg1, TArg2, TArg3, TArg4>(int eventType, Action<TArg1, TArg2, TArg3, TArg4> handler)
        {
            s_EventMgr.Dispatcher.RemoveEventListener(eventType, handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <typeparam name="TArg3">事件参数3类型。</typeparam>
        /// <typeparam name="TArg4">事件参数4类型。</typeparam>
        /// <typeparam name="TArg5">事件参数5类型。</typeparam>
        public static void RemoveEventListener<TArg1, TArg2, TArg3, TArg4, TArg5>(int eventType, Action<TArg1, TArg2, TArg3, TArg4, TArg5> handler)
        {
            s_EventMgr.Dispatcher.RemoveEventListener(eventType, handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        public static void RemoveEventListener(int eventType, Delegate handler)
        {
            s_EventMgr.Dispatcher.RemoveEventListener(eventType, handler);
        }

        //----------------------------string Event----------------------------//
        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <returns></returns>
        public static bool AddEventListener(string eventType, Action handler)
        {
            return s_EventMgr.Dispatcher.AddEventListener(StringId.StringToHash(eventType), handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<TArg1>(string eventType, Action<TArg1> handler)
        {
            return s_EventMgr.Dispatcher.AddEventListener(StringId.StringToHash(eventType), handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<TArg1, TArg2>(string eventType, Action<TArg1, TArg2> handler)
        {
            return s_EventMgr.Dispatcher.AddEventListener(StringId.StringToHash(eventType), handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <typeparam name="TArg3">事件参数3类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<TArg1, TArg2, TArg3>(string eventType, Action<TArg1, TArg2, TArg3> handler)
        {
            return s_EventMgr.Dispatcher.AddEventListener(StringId.StringToHash(eventType), handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <typeparam name="TArg3">事件参数3类型。</typeparam>
        /// <typeparam name="TArg4">事件参数4类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<TArg1, TArg2, TArg3, TArg4>(string eventType, Action<TArg1, TArg2, TArg3, TArg4> handler)
        {
            return s_EventMgr.Dispatcher.AddEventListener(StringId.StringToHash(eventType), handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <typeparam name="TArg3">事件参数3类型。</typeparam>
        /// <typeparam name="TArg4">事件参数4类型。</typeparam>
        /// <typeparam name="TArg5">事件参数5类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<TArg1, TArg2, TArg3, TArg4, TArg5>(string eventType, Action<TArg1, TArg2, TArg3, TArg4, TArg5> handler)
        {
            return s_EventMgr.Dispatcher.AddEventListener(StringId.StringToHash(eventType), handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        public static void RemoveEventListener(string eventType, Action handler)
        {
            s_EventMgr.Dispatcher.RemoveEventListener(StringId.StringToHash(eventType), handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        public static void RemoveEventListener<TArg1>(string eventType, Action<TArg1> handler)
        {
            s_EventMgr.Dispatcher.RemoveEventListener(StringId.StringToHash(eventType), handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        public static void RemoveEventListener<TArg1, TArg2>(string eventType, Action<TArg1, TArg2> handler)
        {
            s_EventMgr.Dispatcher.RemoveEventListener(StringId.StringToHash(eventType), handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <typeparam name="TArg3">事件参数3类型。</typeparam>
        public static void RemoveEventListener<TArg1, TArg2, TArg3>(string eventType, Action<TArg1, TArg2, TArg3> handler)
        {
            s_EventMgr.Dispatcher.RemoveEventListener(StringId.StringToHash(eventType), handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <typeparam name="TArg3">事件参数3类型。</typeparam>
        /// <typeparam name="TArg4">事件参数4类型。</typeparam>
        public static void RemoveEventListener<TArg1, TArg2, TArg3, TArg4>(string eventType, Action<TArg1, TArg2, TArg3, TArg4> handler)
        {
            s_EventMgr.Dispatcher.RemoveEventListener(StringId.StringToHash(eventType), handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <typeparam name="TArg3">事件参数3类型。</typeparam>
        /// <typeparam name="TArg4">事件参数4类型。</typeparam>
        /// <typeparam name="TArg5">事件参数5类型。</typeparam>
        public static void RemoveEventListener<TArg1, TArg2, TArg3, TArg4, TArg5>(string eventType, Action<TArg1, TArg2, TArg3, TArg4, TArg5> handler)
        {
            s_EventMgr.Dispatcher.RemoveEventListener(StringId.StringToHash(eventType), handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        public static void RemoveEventListener(string eventType, Delegate handler)
        {
            s_EventMgr.Dispatcher.RemoveEventListener(StringId.StringToHash(eventType), handler);
        }

        #endregion

        #region 分发消息接口

        public static TArg1 Get<TArg1>()
        {
            return s_EventMgr.GetInterface<TArg1>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        public static void Send(int eventType)
        {
            s_EventMgr.Dispatcher.Send(eventType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="arg1">事件参数1。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        public static void Send<TArg1>(int eventType, TArg1 arg1)
        {
            s_EventMgr.Dispatcher.Send(eventType, arg1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="arg1">事件参数1。</param>
        /// <param name="arg2">事件参数2。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        public static void Send<TArg1, TArg2>(int eventType, TArg1 arg1, TArg2 arg2)
        {
            s_EventMgr.Dispatcher.Send(eventType, arg1, arg2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="arg1">事件参数1。</param>
        /// <param name="arg2">事件参数2。</param>
        /// <param name="arg3">事件参数3。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <typeparam name="TArg3">事件参数3类型。</typeparam>
        public static void Send<TArg1, TArg2, TArg3>(int eventType, TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            s_EventMgr.Dispatcher.Send(eventType, arg1, arg2, arg3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="arg1">事件参数1。</param>
        /// <param name="arg2">事件参数2。</param>
        /// <param name="arg3">事件参数3。</param>
        /// <param name="arg4">事件参数4。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <typeparam name="TArg3">事件参数3类型。</typeparam>
        /// <typeparam name="TArg4">事件参数4类型。</typeparam>
        public static void Send<TArg1, TArg2, TArg3, TArg4>(int eventType, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
        {
            s_EventMgr.Dispatcher.Send(eventType, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType">事件类型。</param>
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
        public static void Send<TArg1, TArg2, TArg3, TArg4, TArg5>(int eventType, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5)
        {
            s_EventMgr.Dispatcher.Send(eventType, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        public static void Send(int eventType, Delegate handler)
        {
            s_EventMgr.Dispatcher.Send(eventType, handler);
        }

        //-------------------------------string Send-------------------------------//
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        public static void Send(string eventType)
        {
            s_EventMgr.Dispatcher.Send(StringId.StringToHash(eventType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="arg1">事件参数1。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        public static void Send<TArg1>(string eventType, TArg1 arg1)
        {
            s_EventMgr.Dispatcher.Send(StringId.StringToHash(eventType), arg1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="arg1">事件参数1。</param>
        /// <param name="arg2">事件参数2。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        public static void Send<TArg1, TArg2>(string eventType, TArg1 arg1, TArg2 arg2)
        {
            s_EventMgr.Dispatcher.Send(StringId.StringToHash(eventType), arg1, arg2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="arg1">事件参数1。</param>
        /// <param name="arg2">事件参数2。</param>
        /// <param name="arg3">事件参数3。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <typeparam name="TArg3">事件参数3类型。</typeparam>
        public static void Send<TArg1, TArg2, TArg3>(string eventType, TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            s_EventMgr.Dispatcher.Send(StringId.StringToHash(eventType), arg1, arg2, arg3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="arg1">事件参数1。</param>
        /// <param name="arg2">事件参数2。</param>
        /// <param name="arg3">事件参数3。</param>
        /// <param name="arg4">事件参数4。</param>
        /// <typeparam name="TArg1">事件参数1类型。</typeparam>
        /// <typeparam name="TArg2">事件参数2类型。</typeparam>
        /// <typeparam name="TArg3">事件参数3类型。</typeparam>
        /// <typeparam name="TArg4">事件参数4类型。</typeparam>
        public static void Send<TArg1, TArg2, TArg3, TArg4>(string eventType, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
        {
            s_EventMgr.Dispatcher.Send(StringId.StringToHash(eventType), arg1, arg2, arg3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType">事件类型。</param>
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
        public static void Send<TArg1, TArg2, TArg3, TArg4, TArg5>(string eventType, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5)
        {
            s_EventMgr.Dispatcher.Send(StringId.StringToHash(eventType), arg1, arg2, arg3, arg4, arg5);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType">事件类型。</param>
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
        public static void Send<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(int eventType, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6)
        {
            s_EventMgr.Dispatcher.Send(eventType, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        public static void Send(string eventType, Delegate handler)
        {
            s_EventMgr.Dispatcher.Send(StringId.StringToHash(eventType), handler);
        }

        #endregion
    }
}