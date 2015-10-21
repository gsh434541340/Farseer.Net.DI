using FS.DI.Core;
using FS.Extends;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FS.DI.Resolver
{
    /// <summary>
    /// 默认解析器实现
    /// </summary>
    internal sealed class DependencyResolver : IDependencyResolver, IScopedResolver
    {
        /// <summary>
        /// 依赖缓存表
        /// </summary>
        private readonly IDependencyTable _dependencyTable;

        /// <summary>
        /// 解析器集合
        /// </summary>
        public ICallSiteCollection CallSiteCollection { get; } = new CallSiteCollection();

        public DependencyResolver(IEnumerable<DependencyEntry> dependencyEntrys)
        {
            if (dependencyEntrys == null) throw new ArgumentNullException(nameof(dependencyEntrys));
            _dependencyTable = new DependencyTable(dependencyEntrys);
            CallSiteCollection.AddDefault(_dependencyTable);
        }

        public DependencyResolver(IDependencyTable dependencyTable)
        {
            if (dependencyTable == null) throw new ArgumentNullException(nameof(dependencyTable));
            _dependencyTable = dependencyTable;
            CallSiteCollection.AddDefault(_dependencyTable);
        }

        public void Dispose()
        {
            if(this is IDependencyResolver)
            {
                _dependencyTable.Dispose();
            }         
            CallSiteCollection.RemoveAll();
        }

        /// <summary>
        /// 解析服务
        /// </summary>
        public Object Resolve(Type serviceType)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));

            DependencyEntry entry;
            if (!_dependencyTable.DependencyEntryTable.TryGetValue(serviceType, out entry))
            {
                throw new InvalidOperationException(string.Format("尝试解析未注册的类型\"{0}\"失败。", serviceType.FullName));
            }
            return BuildUp(new ResolverContext(entry.Last));
        }

        Object IServiceProvider.GetService(Type serviceType)
        {
            return Resolve(serviceType);
        }

        /// <summary>
        /// 创建作用域解析器
        /// </summary>
        /// <returns></returns>
        public IScopedResolver CreateScopedResolver()
        {
            return new DependencyResolver(_dependencyTable);
        }

        /// <summary>
        /// 解析服务集合
        /// </summary>
        public IEnumerable<Object> ResolveAll(Type serviceType)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));

            if (serviceType.IsGenericTypeDefinition)
            {
                var serviceTypes = _dependencyTable.DependencyEntryTable.Values.
                     Where(
                     entry => entry.ServiceType.GetGenericTypeDefinitions().
                     Any(genericType => genericType == serviceType)).
                     Select(entry => entry.ServiceType).
                     ToArray();
                return DistinctBy(BuildUp(serviceTypes), obj => obj.GetType());
            }

            return DistinctBy(BuildUp(serviceType), obj => obj.GetType());
        }

        /// <summary>
        /// 筛选重复的结果
        /// </summary>
        public IEnumerable<TSource> DistinctBy<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        private Object BuildUp(IResolverContext context)
        {
            for (int i = CallSiteCollection.Count - 1; i >= 0; i--)
            {
                var callSite = CallSiteCollection[i];

                if (!callSite.PreResolver(context, this))
                    continue;

                callSite.Resolver(context, this);

                if (!context.Complete)
                    continue;

                return context.CompleteValue;
            }
            return context.CompleteValue;
        }

        private IEnumerable<Object> BuildUp(params Type[] serviceTypes)
        {
            foreach (var serviceType in serviceTypes)
            {
                DependencyEntry entry;
                if (!_dependencyTable.DependencyEntryTable.TryGetValue(serviceType, out entry))
                {
                    throw new InvalidOperationException(string.Format("尝试解析未注册的类型\"{0}\"失败。", serviceType.FullName));
                }
                for (; entry.Next != null; entry = entry.Next)
                {
                    yield return BuildUp(new ResolverContext(entry));
                }
                yield return BuildUp(new ResolverContext(entry));
            }
        }
    }
}