namespace GameFramework.Resource
{
    /// <summary>
    /// 资源运行模式。
    /// </summary>
    public enum ResourceMode: byte
    {
        /// <summary>
        /// 编辑器下的模拟模式。
        /// </summary>
        EditorSimulateMode = 0,
        
        /// <summary>
        /// 离线运行模式。
        /// </summary>
        OfflinePlayMode = 1,
        
        /// <summary>
        /// 联机运行模式。
        /// </summary>
        HostPlayMode = 2,
        
        /// <summary>
        /// WebGL运行模式。
        /// </summary>
        WebPlayMode = 3,
    }
}