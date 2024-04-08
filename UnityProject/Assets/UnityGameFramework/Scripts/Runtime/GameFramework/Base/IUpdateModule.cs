namespace GameFramework
{
    /// <summary>
    /// 游戏框架模块轮询接口。
    /// </summary>
    internal interface IUpdateModule
    {
        
        /// <summary>
        /// 游戏框架模块轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public void Update(float elapseSeconds, float realElapseSeconds);
    }
}