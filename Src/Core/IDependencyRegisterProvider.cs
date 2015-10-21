namespace FS.DI.Core
{
    /// <summary>
    /// 服务注册器提供者
    /// </summary>
    public interface IDependencyRegisterProvider
    {
        /// <summary>
        /// 创建服务注册器
        /// </summary>
        /// <returns></returns>
        IDependencyRegister CreateRegister();
    }
}
