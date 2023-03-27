using System.Collections.Generic;

namespace UnityGameFramework.Runtime
{
    public class BaseBehaviourSingleton
    {
        public bool m_isStart = false;

        public virtual void Awake()
        {
        }

        public virtual bool CanFixedUpdate()
        {
            return false;
        }

        public virtual void Start()
        {
        }
        
        public bool OverrideUpdate = true;
        public virtual void Update()
        {
            OverrideUpdate = false;
        }

        public bool OverrideLateUpdate = true;
        public virtual void LateUpdate()
        {
            OverrideUpdate = false;
        }
        
        public bool OverrideFixedUpdate = true;
        public virtual void FixedUpdate()
        {
            OverrideFixedUpdate = false;
        }
    }

    public class BehaviourSingleton<T> : BaseBehaviourSingleton where T : BaseBehaviourSingleton, new()
    {
        private static T sInstance;

        public static T Instance
        {
            get
            {
                if (null == sInstance)
                {
                    sInstance = new T();
                    sInstance.Awake();
                    RegSingleton(sInstance);
                }

                return sInstance;
            }
        }

        private static void RegSingleton(BaseBehaviourSingleton inst)
        {
            BehaviourSingleSystem.Instance.RegSingleton(inst);
        }
    }

    public class BehaviourSingleSystem : BaseLogicSys<BehaviourSingleSystem>
    {
        List<BaseBehaviourSingleton> m_ListInst = new List<BaseBehaviourSingleton>();
        List<BaseBehaviourSingleton> m_ListStart = new List<BaseBehaviourSingleton>();
        List<BaseBehaviourSingleton> m_ListUpdate = new List<BaseBehaviourSingleton>();
        List<BaseBehaviourSingleton> m_ListFixedUpdate = new List<BaseBehaviourSingleton>();
        List<BaseBehaviourSingleton> m_ListLateUpdate = new List<BaseBehaviourSingleton>();

        public void RegSingleton(BaseBehaviourSingleton inst)
        {
            m_ListInst.Add(inst);
            m_ListStart.Add(inst);
        }

        public override void OnUpdate()
        {
            var listStart = m_ListStart;

            var listToUpdate = m_ListUpdate;
            var listToLateUpdate = m_ListLateUpdate;
            var listToFixedUpdate = m_ListFixedUpdate;

            if (listStart.Count > 0)
            {
                for (int i = 0; i < listStart.Count; i++)
                {
                    var inst = listStart[i];

                    inst.m_isStart = true;
                    inst.Start();
                    
                    if (inst.OverrideUpdate)
                    {
                        listToUpdate.Add(inst);
                    }
                    
                    if (inst.OverrideFixedUpdate)
                    {
                        listToFixedUpdate.Add(inst);
                    }

                    if (inst.OverrideLateUpdate)
                    {
                        listToLateUpdate.Add(inst);
                    }
                }

                listStart.Clear();
            }

            var listUpdateCnt = listToUpdate.Count;
            for (int i = 0; i < listUpdateCnt; i++)
            {
                var inst = listToUpdate[i];

                TProfiler.BeginFirstSample(inst.GetType().FullName);
                inst.Update();
                TProfiler.EndFirstSample();
            }
        }

        public override void OnLateUpdate()
        {
            var listLateUpdate = m_ListLateUpdate;
            var listLateUpdateCnt = listLateUpdate.Count;
            for (int i = 0; i < listLateUpdateCnt; i++)
            {
                var inst = listLateUpdate[i];

                TProfiler.BeginFirstSample(inst.GetType().FullName);
                inst.LateUpdate();
                TProfiler.EndFirstSample();
            }
        }
        
        public override void OnFixedUpdate()
        {
            var listFixedUpdate = m_ListFixedUpdate;
            var listFixedUpdateCnt = listFixedUpdate.Count;
            for (int i = 0; i < listFixedUpdateCnt; i++)
            {
                var inst = listFixedUpdate[i];

                TProfiler.BeginFirstSample(inst.GetType().FullName);
                inst.FixedUpdate();
                TProfiler.EndFirstSample();
            }
        }
    }
}