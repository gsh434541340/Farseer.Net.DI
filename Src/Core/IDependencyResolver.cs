using FS.DI.Resolver;
using System;
using System.Collections.Generic;

namespace FS.DI.Core
{
    /// <summary>
    /// 服务解析器
    /// </summary>
    public interface IDependencyResolver : IServiceProvider, IScopedResolverProvider, IDisposable
    {
        /// <summary>
        /// 解析器集合
        /// </summary>
        ICallSiteCollection CallSiteCollection { get; }

        /// <summary>
        /// 解析服务
        /// </summary>
        Object Resolve(Type serviceType);

        /// <summary>
        /// 解析服务集合
        /// </summary>
        IEnumerable<Object> ResolveAll(Type serviceType);
    } 
}
