using System;

namespace Naruto.Dependency.Abstraction
{
    public interface IIocRegistrar
    {

        /// <summary>
        /// 检查给定的类型是否已经注册
        /// </summary>
        /// <param name="type">待检查的类型</param>
        bool IsRegistered(Type type);

        /// <summary>
        /// 检查给定的类型是否已经注册
        /// </summary>
        /// <typeparam name="T">待检查的类型</typeparam>
        bool IsRegistered<T>();
    }
}
