using GameFramework;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 计时器组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Timer")]
    public sealed partial class TimerComponent : GameFrameworkComponent
    {
        private TimerMgr m_timerManager;

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_timerManager = GameFrameworkEntry.GetModule<TimerMgr>();
            if (m_timerManager == null)
            {
                Log.Fatal("TimerMgr is invalid.");
                return;
            }
        }

        /// <summary>
        /// 添加计时器
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="time"></param>
        /// <param name="isLoop"></param>
        /// <param name="isUnscaled"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public int AddTimer(TimerHandler callback, float time, bool isLoop = false, bool isUnscaled = false, params object[] args)
        {
            if (m_timerManager == null)
            {
                Log.Fatal("TimerMgr is invalid.");
                throw new GameFrameworkException("TimerMgr is invalid.");
            }

            return m_timerManager.AddTimer(callback, time, isLoop, isUnscaled, args);
        }

        /// <summary>
        /// 暂停计时
        /// </summary>
        public void Stop(int timerId)
        {
            if (m_timerManager == null)
            {
                Log.Fatal("TimerMgr is invalid.");
                throw new GameFrameworkException("TimerMgr is invalid.");
            }
        }

        /// <summary>
        /// 恢复计时
        /// </summary>
        public void Resume(int timerId)
        {
            if (m_timerManager == null)
            {
                Log.Fatal("TimerMgr is invalid.");
                throw new GameFrameworkException("TimerMgr is invalid.");
            }
        }

        /// <summary>
        /// 计时器是否在运行中
        /// </summary>
        public bool IsRunning(int timerId)
        {
            if (m_timerManager == null)
            {
                Log.Fatal("TimerMgr is invalid.");
                throw new GameFrameworkException("TimerMgr is invalid.");
            }

            return m_timerManager.IsRunning(timerId);
        }

        /// <summary>
        /// 移除所有计时器
        /// </summary>
        public void RemoveAllTimer()
        {
            if (m_timerManager == null)
            {
                Log.Fatal("TimerMgr is invalid.");
                throw new GameFrameworkException("TimerMgr is invalid.");
            }

            m_timerManager.RemoveAllTimer();
        }
    }
}