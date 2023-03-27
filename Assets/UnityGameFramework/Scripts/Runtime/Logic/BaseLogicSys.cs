namespace UnityGameFramework.Runtime
{
    public class BaseLogicSys<T> : ILogicSys where T : new()
    {
        private static T s_Instance;

        public static bool HasInstance
        {
            get { return s_Instance != null; }
        }

        public static T Instance
        {
            get
            {
                if (null == s_Instance)
                {
                    s_Instance = new T();
                }

                return s_Instance;
            }
        }

        #region virtual fucntion

        public virtual void OnRoleLogout()
        {
        }

        public virtual bool OnInit()
        {
            return true;
        }

        public virtual void OnStart()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnFixedUpdate()
        {
        }
        
        public virtual void OnLateUpdate()
        {
        }

        public virtual void OnDestroy()
        {
        }

        public virtual void OnDrawGizmos()
        {
        }

        public virtual void OnApplicationPause(bool pause)
        {
        }

        #endregion
    }
}