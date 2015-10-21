using FS.DI.Core;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FS.DI.Resolver.CallSite
{
    /// <summary>
    /// 编译解析器
    /// </summary>
    internal sealed class CompileResolverCallSite : IResolverCallSite
    {
        private readonly IDependencyTable _dependencyTable;
        public CompileResolverCallSite(IDependencyTable dependencyTable)
        {
            if (dependencyTable == null) throw new ArgumentNullException(nameof(dependencyTable));
            _dependencyTable = dependencyTable;
        }
        public bool PreResolver(IResolverContext context, IDependencyResolver resolver)
        {
            return context.NotComplete() && context.CompleteValue is Expression;
        }

        public void Resolver(IResolverContext context, IDependencyResolver resolver)
        {
            try
            {
                var factory = _dependencyTable.GetOrAddCompile(context.DependencyEntry,
                    (serviceType, iImplementationType) => CreateDelegate((Expression)context.CompleteValue));
                var completeValue = factory.Invoke(resolver);
                context.CompleteValue = completeValue;
                CacheComplete(context, resolver);
                context.Complete = !_dependencyTable.HasPropertyEntryTable.ContainsKey(context.DependencyEntry);        
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(String.Format("未能创建类型\"{0}\"的实例。", context.DependencyEntry.ServiceType), ex);
            }
        }

        /// <summary>
        /// 编译表达式树生成委托
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        private Func<IDependencyResolver, Object> CreateDelegate(Expression body)
        {
            var parameter = body is InvocationExpression
                ? ((InvocationExpression)body).Arguments.Select(e => (ParameterExpression)e).ToArray()
                : new[] { Expression.Parameter(typeof(IDependencyResolver), "resolver") };
            var lambda = Expression.Lambda<Func<IDependencyResolver, Object>>(body, parameter);
            return lambda.Compile();
        }

        private void CacheComplete(IResolverContext context, IDependencyResolver resolver)
        {
            if (context.IsSingletonLifetime())
            {
                DependencyTableHelper.AddScoped(_dependencyTable, context, null);
            }
            if (context.IsScopedLifetime())
            {
                DependencyTableHelper.AddScoped(_dependencyTable, context, resolver);
            }
        }
    }
}
