using System.Collections.Generic;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Logic")]
    public sealed class LogicComponent : GameFrameworkComponent
    {
        private List<ILogicSys> m_LogicMgrList = new List<ILogicSys>();

        protected override void Awake()
        {
            base.Awake();
            AddLogicSys(BehaviourSingleSystem.Instance);
        }

        public bool AddLogicSys(ILogicSys logicSys)
        {
            if (m_LogicMgrList.Contains(logicSys))
            {
                Log.Fatal("Repeat add logic system: {0}", logicSys.GetType().Name);
                return false;
            }

            if (!logicSys.OnInit())
            {
                Log.Fatal("{0} Init failed", logicSys.GetType().Name);
                return false;
            }

            m_LogicMgrList.Add(logicSys);
          
            logicSys.OnStart();

            return true;
        }

        #region 生命周期
        public void Update()
        {
            TProfiler.BeginFirstSample("Update");

            var listLogic = m_LogicMgrList;
            var logicCnt = listLogic.Count;
            for (int i = 0; i < logicCnt; i++)
            {
                var logic = listLogic[i];
                TProfiler.BeginSample(logic.GetType().FullName);
                logic.OnUpdate();
                TProfiler.EndSample();
            }

            TProfiler.EndFirstSample();
        }
        
        public void FixedUpdate()
        {
            TProfiler.BeginFirstSample("FixedUpdate");

            var listLogic = m_LogicMgrList;
            var logicCnt = listLogic.Count;
            for (int i = 0; i < logicCnt; i++)
            {
                var logic = listLogic[i];
                TProfiler.BeginSample(logic.GetType().FullName);
                logic.OnFixedUpdate();
                TProfiler.EndSample();
            }

            TProfiler.EndFirstSample();
        }

        public void LateUpdate()
        {
            TProfiler.BeginFirstSample("LateUpdate");
            var listLogic = m_LogicMgrList;
            var logicCnt = listLogic.Count;
            for (int i = 0; i < logicCnt; i++)
            {
                var logic = listLogic[i];
                TProfiler.BeginSample(logic.GetType().FullName);
                logic.OnLateUpdate();
                TProfiler.EndSample();
            }

            TProfiler.EndFirstSample();
        }


        public void RoleLogout()
        {
            var listLogic = m_LogicMgrList;
            var logicCnt = listLogic.Count;
            for (int i = 0; i < logicCnt; i++)
            {
                var logic = listLogic[i];
                logic.OnRoleLogout();
            }
        }

        public void OnDrawGizmos()
        {
#if UNITY_EDITOR
            var listLogic = m_LogicMgrList;
            var logicCnt = listLogic.Count;
            for (int i = 0; i < logicCnt; i++)
            {
                var logic = listLogic[i];
                logic.OnDrawGizmos();
            }
#endif
        }

        public void OnDestroy()
        {
            var listLogic = m_LogicMgrList;
            var logicCnt = listLogic.Count;
            for (int i = 0; i < logicCnt; i++)
            {
                var logic = listLogic[i];
                logic.OnDestroy();
            }
        }

        public void OnApplicationPause(bool pause)
        {
            var listLogic = m_LogicMgrList;
            var logicCnt = listLogic.Count;
            for (int i = 0; i < logicCnt; i++)
            {
                var logic = listLogic[i];
                logic.OnApplicationPause(pause);
            }
        }

        #endregion
    }
}