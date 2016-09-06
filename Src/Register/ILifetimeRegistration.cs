namespace FS.DI.Register
{
    /// <summary>
    /// 依赖注入对象的生命周期配置
    /// </summary>
    /// <typeparam name="TRegistration"></typeparam>
    public interface ILifetimeRegistration<out TRegistration>
    {
        /// <summary>
        /// 注册为瞬态实例的生命周期
        /// </summary>
        /// <returns></returns>
        TRegistration AsTransientLifetime();

        /// <summary>
        /// 注册为作用域的生命周期
        /// </summary>
        /// <returns></returns>
        TRegistration AsScopedLifetime();

        /// <summary>
        /// 注册为单例的生命周期
        /// </summary>
        /// <returns></returns>
        TRegistration AsSingletonLifetime();
    }
}
