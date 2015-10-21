using FS.DI.Core;
using System.Linq.Expressions;

namespace FS.DI.Resolver.CallSite
{
    /// <summary>
    /// 实例解析器调用
    /// </summary>
    internal sealed class InstanceResolverCallSite : IResolverCallSite
    {
        public bool PreResolver(IResolverContext context, IDependencyResolver resolver)
        {
            return context.NotComplete() && context.HasImplementationInstance();
        }

        public void Resolver(IResolverContext context, IDependencyResolver resolver)
        {
            context.CompleteValue = Expression.Constant(
                context.DependencyEntry.ImplementationInstance,
                context.DependencyEntry.ServiceType);
        }
    }
}
