using FS.DI.Core;
using System;
using System.Collections.Generic;

namespace FS.DI.Resolver
{
    /// <summary>
    /// 依赖缓存表
    /// </summary>
    internal interface IDependencyTable : IDisposable
    {
        IDictionary<Type, DependencyEntry> DependencyEntryTable { get; }

        IDictionary<Type, DependencyEntry> PropertyEntryTable { get; }

        IDictionary<DependencyEntry, bool> HasPropertyEntryTable { get; }

        IDictionary<Tuple<IScopedResolver, Type, Type>, Object> ScopedTable { get; }

        IDictionary<Tuple<Type, Type>, Func<IDependencyResolver, Object>> CompileTable { get; }
    }
}
  