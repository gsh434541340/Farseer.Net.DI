using FS.DI.Core;

namespace FS.DI.Register
{
    /// <summary>
    /// 依赖服务注册实现类
    /// </summary>
    internal sealed class DependencyRegistration : IDependencyRegistration
    {
        internal DependencyEntry Entry { get; set; }

        public DependencyRegistration(DependencyEntry entry)
        {
            Entry = entry;
        }

        /// <summary>
        /// 注册为瞬态实例的生命周期
        /// </summary>
        /// <returns></returns>
        public IDependencyRegistration AsTransientLifetime()
        {
            Entry.Lifetime = DependencyLifetime.Transient;
            return this;
        }

        /// <summary>
        /// 注册为作用域的生命周期
        /// </summary>
        /// <returns></returns>
        public IDependencyRegistration AsScopedLifetime()
        {
            Entry.Lifetime = DependencyLifetime.Scoped;
            return this;
        }

        /// <summary>
        /// 注册为单例的生命周期
        /// </summary>
        /// <returns></returns>
        public IDependencyRegistration AsSingletonLifetime()
        {
            Entry.Lifetime = DependencyLifetime.Singleton;
            return this;
        }

        public IDependencyRegistration AsPropertyDependency()
        {
            Entry.Style = DependencyStyle.PropertyDependency;
            return this;
        }
    }
}
