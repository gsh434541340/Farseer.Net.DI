using FS.DI.Resolver.CallSite;

namespace FS.DI.Resolver
{
    /// <summary>
    /// 解析器集合帮助类
    /// </summary>
    internal static class CallSiteCollectionHelper
    {
        /// <summary>
        /// 添加默认解析器实现
        /// </summary>
        internal static ICallSiteCollection AddDefault(this ICallSiteCollection callSiteCollection, IDependencyTable dependencyTable)
        {
            callSiteCollection.Add(new PropertyResolverCallSite(dependencyTable));
            callSiteCollection.Add(new CompileResolverCallSite(dependencyTable));
            callSiteCollection.Add(new ConstructorResolverCallSite(dependencyTable));
            callSiteCollection.Add(new NonConstructorResolverCallSite());
            callSiteCollection.Add(new InstanceResolverCallSite());
            callSiteCollection.Add(new DelegateResolverCallSite());
            callSiteCollection.Add(new ScopedResolverCallSite(dependencyTable));
            callSiteCollection.Add(new SingletonResolverCallSite(dependencyTable));
            callSiteCollection.Add(new TransientResolverCallSite(dependencyTable));
            return callSiteCollection;
        }
    }
}
