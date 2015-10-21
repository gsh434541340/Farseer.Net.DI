using FS.DI.Core;
using System.Linq.Expressions;

namespace FS.DI.Resolver.CallSite
{
    /// <summary>
    /// 无构造方法解析器调用
    /// </summary>
    internal sealed class NonConstructorResolverCallSite : IResolverCallSite
    {
        public bool PreResolver(IResolverContext context, IDependencyResolver resolver)
        {
            return context.NotComplete() && context.HasImplementationType() && !context.HasPublicConstructor();
        }

        public void Resolver(IResolverContext context, IDependencyResolver resolver)
        {
            context.CompleteValue = Expression.New(context.DependencyEntry.ImplementationType);
        }
    }
}
