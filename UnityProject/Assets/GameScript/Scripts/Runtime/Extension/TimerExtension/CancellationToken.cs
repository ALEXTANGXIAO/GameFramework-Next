using System;
using System.Collections.Generic;
using UGFExtensions.Timer;
using UnityGameFramework.Runtime;

namespace UGFExtensions
{
    public class CancellationToken
    {
        private HashSet<Action> m_Actions = new HashSet<Action>();

        public void Add(Action callback)
        {
            // 如果action是null，绝对不能添加,要抛异常，说明有协程泄漏
            m_Actions.Add(callback);
        }
        
        public void Remove(Action callback)
        {
            m_Actions?.Remove(callback);
        }

        public void Cancel()
        {
            if (m_Actions == null)
            {
                return;
            }
            
            if (m_Actions.Count == 0)
            {
                return;
            }

            Invoke();
        }

        private void Invoke()
        {
            HashSet<Action> runActions = m_Actions;
            m_Actions = null;
            try
            {
                foreach (Action action in runActions)
                {
                    action.Invoke();
                }
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }

        public async void CancelAfter(long afterTimeCancel)
        {
            if (m_Actions == null)
            {
                return;
            }

            if (m_Actions.Count == 0)
            {
                return;
            }

            await GameModule.Timer.OnceTimerAsync(afterTimeCancel);
            
            Invoke();
        }
    }
}