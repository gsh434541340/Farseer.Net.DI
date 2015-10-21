using FS.DI.Core;
using System;

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
        internal static Tuple<IScopedResolver, Type, Type> GetScopedKey(IResolverContext context, IDependencyResolver resolver)
        {
            return new Tuple<IScopedResolver, Type, Type>((IScopedResolver)resolver,
               context.DependencyEntry.ServiceType,
               context.DependencyEntry.GetImplementationType());
        }

        /// <summary>
        /// 获取委托缓存的key
        /// </summary>
        internal static Tuple<Type, Type> GetCompileKey(DependencyEntry depencyEntry)
        {
            return new Tuple<Type, Type>(depencyEntry.ServiceType, depencyEntry.GetImplementationType());
        }

        /// <summary>
        /// 获取或添加委托缓存
        /// </summary>
        internal static Func<IDependencyResolver, Object> GetOrAddCompile(this IDependencyTable dependencyTable,
                     DependencyEntry depencyEntry, Func<Type, Type, Func<IDependencyResolver, Object>> valueFactory)
        {
            if (depencyEntry == null) throw new ArgumentNullException(nameof(depencyEntry));

            var key = DependencyTableHelper.GetCompileKey(depencyEntry);
            Func<IDependencyResolver, Object> resultingValue;
            if (dependencyTable.CompileTable.TryGetValue(key, out resultingValue))
            {
                return resultingValue;
            }
            return (dependencyTable.CompileTable[key] = valueFactory(key.Item1, key.Item2));
        }

        /// <summary>
        /// 从缓存中读取委托并调用
        /// </summary>
        internal static bool TryGetCompileValue(this IDependencyTable dependencyTable, IResolverContext context, IDependencyResolver resolver)
        {
            Func<IDependencyResolver, Object> resultingValueFactory;
            if (dependencyTable.CompileTable.TryGetValue(DependencyTableHelper.GetCompileKey(context.DependencyEntry), out resultingValueFactory))
            {
                context.CompleteValue = resultingValueFactory(resolver);
                if (dependencyTable.HasPropertyEntryTable.ContainsKey(context.DependencyEntry))
                {
                    new PropertyResolverCallSite().Resolver(context, resolver);
                }
                return true;
            }
            return false;
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
