﻿using System;

namespace FS.DI.Core
{
    /// <summary>
    /// 忽略属性依赖
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreDependencyAttribute : Attribute
    {
    } 
}
