﻿using FS.DI.Core;
using FS.Extends;
using System;
using System.Linq;
using System.Reflection;
 
namespace FS.DI.Resolver.CallSite
{
    /// <summary>
    /// CallSite帮助类
    /// </summary>
    internal static class CallSiteHelper
    {
        /// <summary>
        /// 是否执行完成
        /// </summary>
        internal static bool NotComplete(this IResolverContext context)
        {
            return !context.Complete;
        }

        /// <summary>
        /// 是否Transient生命周期
        /// </summary>
        internal static bool IsTransientLifetime(this IResolverContext context)
        {
            return context.DependencyEntry.Lifetime == DependencyLifetime.Transient;
        }

        /// <summary>
        /// 是否Singleton生命周期
        /// </summary>
        internal static bool IsSingletonLifetime(this IResolverContext context)
        {
            return context.DependencyEntry.Lifetime == DependencyLifetime.Singleton;
        }

        /// <summary>
        /// 是否Scoped生命周期
        /// </summary>
        internal static bool IsScopedLifetime(this IResolverContext context)
        {
            return context.DependencyEntry.Lifetime == DependencyLifetime.Scoped;
        }

        /// <summary>
        /// ImplementationType是否有值
        /// </summary>
        internal static bool HasImplementationType(this IResolverContext context)
        {
            return context.DependencyEntry.ImplementationType != null;
        }

        /// <summary>
        /// ImplementationInstance是否有值
        /// </summary>
        internal static bool HasImplementationInstance(this IResolverContext context)
        {
            return context.DependencyEntry.ImplementationInstance != null;
        }

        /// <summary>
        /// ImplementationDelegate是否有值
        /// </summary>
        internal static bool HasImplementationDelegate(this IResolverContext context)
        {
            return context.DependencyEntry.ImplementationDelegate != null;
        }

        /// <summary>
        /// 是否含有公共的构造方法
        /// </summary>
        internal static bool HasPublicConstructor(this IResolverContext context)
        {
            return context.DependencyEntry.GetImplementationType().GetConstructors().Any(ctor => ctor.GetParameters().Length > 0);
        }

        /// <summary>
        /// 返回最佳构造方法
        /// </summary>
        internal static ConstructorInfo GetBastConstructor(this Type type, IDependencyTable dependencyTable)
        {
            var constructors = type.GetConstructors().OrderBy(ctor => ctor.GetParameters().Length).ToArray();
            if (constructors.Length == 0)
            {
                throw new InvalidOperationException(type.FullName + "类没有公共的构造方法。");
            }
            else if (constructors.Length == 1)
            {
                return constructors[0];
            }
            else
            {
                ConstructorInfo bestConstructor = null;
                foreach (var constructor in constructors)
                {
                    if (!constructor.GetParameterTypes().Any(t => !dependencyTable.DependencyEntryTable.ContainsKey(t)))
                    {
                        if (bestConstructor == null)
                        {
                            bestConstructor = constructor;
                        }
                        else
                        {
                            if (bestConstructor.GetParameters().Length == constructor.GetParameters().Length)
                            {
                                throw new InvalidOperationException("类型\"" + type.FullName + "\" 构造方法调用不明确。");
                            }
                            bestConstructor = constructor;
                        }
                    }
                }
                if (bestConstructor == null)
                {
                    throw new InvalidOperationException("类型\"" + type.FullName + "\"未找到合适的构造方法。");
                }
                return bestConstructor;
            }
            throw new InvalidOperationException("类型\"" + type.FullName + "\"未找到合适的构造方法。");
        }

        internal static Object[] GetConstructorParameters(this Type type, IDependencyTable dependencyTable, IDependencyResolver resolver)
        {
            return type.GetBastConstructor(dependencyTable).
                    GetParameters().
                    Select(p => resolver.Resolve(p.ParameterType)).
                    ToArray();
        }
    }
}