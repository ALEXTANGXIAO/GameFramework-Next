using GameFramework;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 数据中心组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/DataCenter")]
    public class DataCenterComponent:GameFrameworkComponent
    {
        /// <summary>
        /// 数据中心系统。
        /// </summary>
        private IGameModule m_DataCenterSys;

        protected override void Awake()
        {
            base.Awake();
            m_DataCenterSys = DataCenterSys.Instance;
        }
    }
}