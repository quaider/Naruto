﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Dependency.Abstraction
{
    /// <summary>
    /// 实现此接口的类型将自动注册为 <see cref="LifetimeStyle.Transient" /> 模式
    /// </summary>
    public interface ITransientDependency
    {
    }
}
