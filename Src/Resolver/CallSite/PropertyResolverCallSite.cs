using FS.Cache;
using FS.DI.Core;
using System;
using System.Reflection;

namespace FS.DI.Resolver.CallSite
{
    /// <summary>
    /// 属性注入解析器调用
    /// </summary>
    internal sealed class PropertyResolverCallSite : IResolverCallSite
    {
        public bool PreResolver(IResolverContext context, IDependencyResolver resolver)
        {
            return context.NotComplete();
        }

        public void Resolver(IResolverContext context, IDependencyResolver resolver)
        {
            var properties = context.CompleteValue.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                try
                {
                    PropertySetCacheManger.Cache(property, context.CompleteValue, resolver.Resolve(property.PropertyType));
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(String.Format("类型\"{0}\"未能注入属性\"{1}\"的实例。",
                        context.DependencyEntry.GetImplementationType(), property.PropertyType), ex);
                }
            }
            context.Complete = true;
        }
    }
}
