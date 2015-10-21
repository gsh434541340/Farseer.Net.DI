using FS.DI.Core;
using System;

namespace FS.DI.Register
{
    /// <summary>
    /// 服务注册器实现类
    /// </summary>
    internal class DependencyRegister : IDependencyRegister
    {
        private readonly IFarseerContainer _container;

        internal DependencyRegister(IFarseerContainer container)
        {
            _container = container;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="dependencyEntry"></param>
        public void RegisterEntry(DependencyEntry dependencyEntry)
        {
            if (dependencyEntry == null)
                throw new ArgumentNullException(nameof(dependencyEntry));

            _container.Add(dependencyEntry);
        }   
    }
}
