using System;
using System.Collections.Generic;
using Naruto.Dependency.Abstraction;
using Autofac;
using Naruto.Dependency.Extensions;

namespace Naruto.Dependency
{
    public class IocManager : IIocManager
    {
        public static IocManager Instance { get; private set; }

        internal ContainerBuilder ContainerBuilder;
        protected IContainer Container;

        static IocManager()
        {
            Instance = new IocManager();
        }

        private IocManager()
        {
            ContainerBuilder = new ContainerBuilder();
            RegisterInstance<IIocManager>(this, LifetimeStyle.Singleton);
        }

        internal IContainer Build()
        {
            return Container ?? (Container = ContainerBuilder.Build());
        }

        #region Resolve

        public T Resolve<T>()
        {
            var type = typeof(T);
            return Container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return Container.Resolve(type);
        }

        public T Resolve<T>(Type type)
        {
            return (T)Container.Resolve(type);
        }

        public T Resolve<T>(string name)
        {
            return Container.ResolveNamed<T>(name);
        }

        //todo 待测试
        public IEnumerable<T> ResolveAll<T>()
        {
            return Container.Resolve<IEnumerable<T>>();
        }

        //todo 待测试
        public IEnumerable<object> ResolveAll(Type type)
        {
            var genericType = typeof(IEnumerable<>).MakeGenericType(type);
            return Container.Resolve(genericType) as IEnumerable<object>;
        }

        #endregion

        #region Register

        public void Register<TService>(LifetimeStyle lifetime)
        {
            ContainerBuilder.RegisterType<TService>().AsLifeTime(lifetime);
        }

        public void Register(Type type, LifetimeStyle lifetime)
        {
            ContainerBuilder.RegisterType(type).AsLifeTime(lifetime);
        }

        public void Register<TService, TImpl>(LifetimeStyle lifetime)
        {
            ContainerBuilder.RegisterType<TImpl>().As<TService>().AsLifeTime(lifetime);
        }

        public void Register(Type service, Type impl, LifetimeStyle lifetime)
        {
            ContainerBuilder.RegisterType(impl).As(service).AsLifeTime(lifetime);
        }

        public void Register<TService>(string name, LifetimeStyle lifetime)
        {
            ContainerBuilder.RegisterType<TService>().Named<TService>(name).AsLifeTime(lifetime);
        }

        public void RegisterInstance<TService>(TService instance, LifetimeStyle lifetime) where TService : class
        {
            ContainerBuilder.RegisterInstance(instance).AsSelf().AsLifeTime(lifetime);
        }

        public void RegisterInstance<TService>(object instance, LifetimeStyle lifetime) where TService : class
        {
            var convert = instance as TService;
            if (convert == null) throw new InvalidCastException($"the parameter `{nameof(instance)}` can not be cast to type `{typeof(TService)}` ");
            ContainerBuilder.RegisterInstance(convert).AsSelf().AsLifeTime(lifetime);
        }

        public void RegisterInstance<TService>(string name, object instance, LifetimeStyle lifetime) where TService : class
        {
            ContainerBuilder.RegisterInstance(instance).Named<TService>(name).AsLifeTime(lifetime);
        }

        public void Register<TService>(Func<IComponentContext, TService> factory, LifetimeStyle lifetime)
        {
            ContainerBuilder.Register(ctx => factory(ctx)).AsLifeTime(lifetime);
        }

        #endregion

        public bool IsRegistered(Type type)
        {
            return Container.IsRegistered(type);
        }

        public bool IsRegistered<T>()
        {
            return Container.IsRegistered<T>();
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}
