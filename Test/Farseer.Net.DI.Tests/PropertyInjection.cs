﻿using FS.DI.Core;
using FS.DI.Tests.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FS.DI.Tests
{
    [TestClass]
    public class PropertyInjection
    {
        /// <summary>
        /// 初始化容器
        /// </summary>
        private readonly IFarseerContainer container = new FarseerContainer();

        [TestMethod]
        public void AutoPropertyInjection()
        {
            ///创建注册器
            IDependencyRegister register = container.CreateRegister();
            ///注册类型
            register.RegisterType<IRepository<UserEntity>, UserRepository>();
            register.RegisterType<IUserService, UserService>();
            ///注册自动属性
            register.RegisterType<ILogger, Logger>().AsPropertyDependency();

            ///创建解析器
            using (IDependencyResolver resolver = container.CreateResolver())
            {
                IRepository<UserEntity> userRepository = resolver.Resolve<IRepository<UserEntity>>();
                var logger = ((UserRepository)userRepository).Logger;

                Assert.IsInstanceOfType(logger, typeof(Logger));
                Assert.AreNotEqual<ILogger>(logger, UnKownLogger.Instance);

                IUserService useService = resolver.Resolve<IUserService>();
                Assert.AreEqual<ILogger>(((UserService)useService).Logger, UnKownLogger.Instance);
            }
        }
    }
}
