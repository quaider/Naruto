﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Dependency.Abstraction
{
    /// <summary>
    /// 表示依赖注入的对象生命周期
    /// </summary>
    public enum LifetimeStyle
    {
        /// <summary>
        /// 实时模式，每次获取都创建不同对象
        /// </summary>
        Transient,

        /// <summary>
        /// 局部模式，同一生命周期获得相同对象，不同生命周期获得不同对象
        /// </summary>
        Scoped,

        /// <summary>
        /// 单例模式
        /// </summary>
        Singleton
    }
}
