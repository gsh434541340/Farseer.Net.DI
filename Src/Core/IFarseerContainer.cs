using System.Collections;
using System.Collections.Generic;

namespace FS.DI.Core
{
    /// <summary>
    /// Farseer.IoC容器
    /// </summary>
    public interface IFarseerContainer : IEnumerable<DependencyEntry>, IDependencyRegisterProvider, IDependencyResolverProvider, IEnumerable
    {
        /// <summary>
        ///  获取容器中包含的元素数
        /// </summary>
        int Count { get; }
        /// <summary>
        /// 将依赖注入对象添加到容器中
        /// </summary>
        /// <param name="dependencyEntry"></param>
        void Add(DependencyEntry dependencyEntry);
        /// <summary>
        /// 深拷贝容器
        /// </summary>
        /// <returns></returns>
        IFarseerContainer Clone();
        /// <summary>
        /// 从容器中移除所有对象
        /// </summary>
        void Clear();
        /// <summary>
        /// 设置服务注册器提供者
        /// </summary>
        /// <param name="registerProvider"></param>
        void SetRegisterProvider(IDependencyRegisterProvider registerProvider);
    }
}
