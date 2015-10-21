namespace FS.DI.Core
{
    /// <summary>
    /// 服务注册器
    /// </summary>
    public interface IDependencyRegister
    {
        /// <summary>
        /// 依赖注入注册
        /// </summary>
        void RegisterEntry(DependencyEntry dependencyEntry);
    }
}
