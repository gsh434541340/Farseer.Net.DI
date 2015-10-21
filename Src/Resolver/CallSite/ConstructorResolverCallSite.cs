using FS.DI.Core;
using FS.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FS.DI.Resolver.CallSite
{
    /// <summary>
    /// 构造方法解析器调用
    /// </summary>
    internal sealed class ConstructorResolverCallSite : IResolverCallSite
    {
        private readonly IDependencyTable _dependencyTable;
        public ConstructorResolverCallSite(IDependencyTable dependencyTable)
        {
            if (dependencyTable == null) throw new ArgumentNullException(nameof(dependencyTable));
            _dependencyTable = dependencyTable;
        }
        public bool PreResolver(IResolverContext context, IDependencyResolver resolver)
        {
            return context.NotComplete() && context.HasImplementationType() && context.HasPublicConstructor();
        }

        public void Resolver(IResolverContext context, IDependencyResolver resolver)
        {
            var implType = context.DependencyEntry.ImplementationType;
            var constructor = implType.GetBastConstructor(_dependencyTable);

            if (constructor == null) throw new InvalidOperationException(implType.FullName + "不存在公共构造方法。");

            context.CompleteValue = Expression.New(constructor, GetConstructorParameters(constructor, resolver));
        }

        /// <summary>
        /// 创建构造方法参数表达式树集合
        /// </summary>
        /// <param name="constructor"></param>
        /// <returns></returns>
        private IEnumerable<Expression> GetConstructorParameters(ConstructorInfo constructor, IDependencyResolver resolver)
        {
            var lambdaParam = Expression.Parameter(typeof(object[]), "args");
            return constructor.GetParameterTypes().Select(parameterType => 
                 Expression.Constant(resolver.Resolve(parameterType), parameterType));
        }
    }
}
