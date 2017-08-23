using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Dependency.Abstraction
{
    /// <summary>
    /// 依赖注入对象解析器
    /// </summary>
    public interface IIocResolver
    {
        /// <summary>
        /// 获取指定类型的实例
        /// </summary>
        /// <typeparam name="T">待获取对象的类型</typeparam>
        /// <returns>已获取的指定类型的对象</returns>
        T Resolve<T>();

        /// <summary>
        /// 获取指定类型的实例
        /// </summary>
        /// <param name="type">待获取对象的类型</param>
        /// <returns>object实例</returns>
        object Resolve(Type type);

        /// <summary>
        /// 获取指定类型的实例
        /// </summary>
        /// <typeparam name="T">需转换的目标类型</typeparam>
        /// <param name="type">待获取对象的类型</param>
        /// <returns>返回转换后的指定类型的实例</returns>
        T Resolve<T>(Type type);

        /// <summary>
        /// 获取指定名称的实例
        /// </summary>
        /// <typeparam name="T">实例的类型</typeparam>
        /// <param name="name">注册时提供的名称</param>
        T Resolve<T>(string name);

        /// <summary>
        /// 获取指定类型的所有实例
        /// </summary>
        /// <typeparam name="T">待获取对象的类型</typeparam>
        /// <returns>待获取对象的所有实例</returns>
        IEnumerable<T> ResolveAll<T>();

        /// <summary>
        /// 获取指定类型的所有实例
        /// </summary>
        /// <param name="type">待获取对象的类型</param>
        /// <returns>待获取对象的所有实例的object形式</returns>
        IEnumerable<object> ResolveAll(Type type);
    }
}
