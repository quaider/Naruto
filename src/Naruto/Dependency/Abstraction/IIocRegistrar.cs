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

        void Register<TService>(LifetimeStyle lifetime);

        void Register<TService>(string name, LifetimeStyle lifetime);

        void Register(Type type, LifetimeStyle lifetime);

        void Register<TService, TImpl>(LifetimeStyle lifetime);

        void Register(Type service, Type impl, LifetimeStyle lifetime);

        void RegisterInstance<TService>(TService instance, LifetimeStyle lifetime) where TService : class;

        void RegisterInstance<TService>(object instance, LifetimeStyle lifetime) where TService : class;

        void RegisterInstance<TService>(string name, object instance, LifetimeStyle lifetime) where TService : class;
    }
}
