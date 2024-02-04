namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 全局单例对象（非线程安全）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static T s_Instance = default(T);

        public static T Instance
        {
            get
            {
                if (null == s_Instance)
                {
                    s_Instance = new T();
                    s_Instance.Init();
                }

                return s_Instance;
            }
        }

        public static bool IsValid => s_Instance != null;

        protected Singleton()
        {
        }

        protected virtual void Init()
        {
        }

        public virtual void Active()
        {
        }

        protected virtual void OnRelease()
        {
        }

        public virtual void Release()
        {
            s_Instance.OnRelease();
            s_Instance = null;
        }
    }
}