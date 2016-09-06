﻿using FS.DI.Core;
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

            var parameter = Expression.Parameter(typeof(object[]), "args");
            var body = Expression.New(constructor, GetConstructorParameters(constructor, parameter));
            var factory = Expression.Lambda<Func<IDependencyResolver, Object[], Object>>(body,
               Expression.Parameter(typeof(IDependencyResolver)),
               parameter);
            context.CompleteValue = factory;
        }

        /// <summary>
        /// 创建构造方法参数表达式树集合
        /// </summary>
        /// <param name="constructor"></param>
        /// <returns></returns>
        private IEnumerable<Expression> GetConstructorParameters(ConstructorInfo constructor, ParameterExpression parameter)
        {
            
            return constructor.GetParameterTypes().Select((parameterType, index) =>
                   Expression.Convert(Expression.ArrayIndex(
                       parameter, Expression.Constant(index)),
                       parameterType));
        }
    }
}
