namespace FS.DI.Core
{
    /// <summary>
    /// 服务解析器提供者
    /// </summary>
    public interface IDependencyResolverProvider
    {
        /// <summary>
        /// 创建服务解析器
        /// </summary>
        /// <returns></returns>
        IDependencyResolver CreateResolver();
    }
}
