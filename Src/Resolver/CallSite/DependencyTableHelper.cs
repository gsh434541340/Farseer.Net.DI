using FS.DI.Core;
using System;
using System.Linq;
 
namespace FS.DI.Resolver.CallSite
{
    /// <summary>
    /// DependencyTable帮助类
    /// </summary>
    internal static class DependencyTableHelper
    {
        /// <summary>
        /// 获取作用域缓存的key
        /// </summary>
        internal static Tuple<IScopedResolver, DependencyEntry> GetScopedKey(IResolverContext context, IDependencyResolver resolver)
        {
            return new Tuple<IScopedResolver, DependencyEntry>((IScopedResolver)resolver, context.DependencyEntry);
        }

        /// <summary>
        /// 获取或添加委托缓存
        /// </summary>
        internal static Func<IDependencyResolver, Object[], Object> GetOrAddCompile(this IDependencyTable dependencyTable,
                     DependencyEntry depencyEntry, Func<Type, Type, Func<IDependencyResolver, Object[], Object>> valueFactory)
        {
            if (depencyEntry == null) throw new ArgumentNullException(nameof(depencyEntry));

            Func<IDependencyResolver, Object[], Object> resultingValue;
            if (dependencyTable.CompileTable.TryGetValue(depencyEntry, out resultingValue))
            {
                return resultingValue;
            }
            return (dependencyTable.CompileTable[depencyEntry] = valueFactory(depencyEntry.ServiceType, depencyEntry.GetImplementationType()));
        }

        /// <summary>
        /// 从缓存中读取委托并调用
        /// </summary>
        internal static bool TryGetCompileValue(this IDependencyTable dependencyTable, IResolverContext context, IDependencyResolver resolver)
        {
            Func<IDependencyResolver,Object[], Object> resultingValueFactory;
            if (dependencyTable.CompileTable.TryGetValue(context.DependencyEntry, out resultingValueFactory))
            {
                var args = context.DependencyEntry.GetImplementationType().
                    GetConstructorParameters(dependencyTable, resolver);              
                context.CompleteValue = resultingValueFactory(resolver, args);
                if (dependencyTable.HasPropertyEntryTable.ContainsKey(context.DependencyEntry))
                {
                    new PropertyResolverCallSite(dependencyTable).Resolver(context, resolver);
                }        
            }
            return context.Complete;
        }

        /// <summary>
        /// 从缓存中读取作用域值
        /// </summary>
        internal static bool TryGetScoped(this IDependencyTable dependencyTable, IResolverContext context, IDependencyResolver resolver, out Object value)
        {
            return dependencyTable.ScopedTable.TryGetValue(DependencyTableHelper.GetScopedKey(context, resolver), out value);
        }

        internal static void AddScoped(this IDependencyTable dependencyTable,IResolverContext context, IDependencyResolver resolver)
        {
            dependencyTable.ScopedTable.Add(DependencyTableHelper.GetScopedKey(context, resolver), context.CompleteValue);
        }
    }
}
