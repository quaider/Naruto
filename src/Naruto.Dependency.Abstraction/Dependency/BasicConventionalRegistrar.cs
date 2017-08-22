using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Naruto.Dependency.Abstraction;
using Naruto.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Naruto.Dependency
{
    public class BasicConventionalRegistrar
    {
        public void Register(IServiceCollection collection)
        {
            var finder = new AppDomainTypeFinder();

            //添加即时生命周期类型的映射
            var dependencyTypes = finder.OfType<ITransientDependency>().Distinct();
            AddTypeWithInterfaces(collection, dependencyTypes, LifetimeStyle.Transient);

            //添加局部生命周期类型的映射
            dependencyTypes = finder.OfType<IScopeDependency>().Distinct();
            AddTypeWithInterfaces(collection, dependencyTypes, LifetimeStyle.Scoped);

            //添加单例生命周期类型的映射
            dependencyTypes = finder.OfType<ISingletonDependency>().Distinct();
            AddTypeWithInterfaces(collection, dependencyTypes, LifetimeStyle.Singleton);
        }

        /// <summary>
        /// 以类型实现的接口进行服务添加，需排除
        /// <see cref="ITransientDependency" />、
        /// <see cref="IScopeDependency" />、
        /// <see cref="ISingletonDependency" />、
        /// <see cref="IDependency" />、
        /// <see cref="IDisposable" />等非业务接口，如无接口则注册自身
        /// </summary>
        /// <param name="components">服务映射信息集合</param>
        /// <param name="dependencyTypes">要注册的实现类型集合</param>
        /// <param name="lifetime">注册的生命周期类型</param>
        protected virtual void AddTypeWithInterfaces(IServiceCollection collection, IEnumerable<Type> dependencyTypes,
            LifetimeStyle lifetime)
        {
            foreach (var implType in dependencyTypes)
            {
                if (implType.IsAbstract || implType.IsInterface)
                {
                    continue;
                }

                ServiceDescriptor serviceDescriptor;
                var serviceTypes = GetServiceTypes(implType);
                if (serviceTypes.Length == 0)
                {
                    serviceDescriptor = new ServiceDescriptor(implType, implType, CastToMsLifeTime(lifetime));
                    collection.TryAdd(serviceDescriptor);
                    continue;
                }

                foreach (var serviceType in serviceTypes)
                {
                    serviceDescriptor = new ServiceDescriptor(serviceType, implType, CastToMsLifeTime(lifetime));
                    collection.TryAdd(serviceDescriptor);
                }
            }
        }

        //bug should be fixed later
        private Type[] GetServiceTypes(Type type)
        {
            Type[] exceptInterfaces =
            {
                typeof (IDisposable),
                typeof (ITransientDependency),
                typeof (IScopeDependency),
                typeof (ISingletonDependency)
            };

            var serviceTypes = type.GetInterfaces().Where(m => !exceptInterfaces.Contains(m)).ToList();

            for (var index = 0; index < serviceTypes.Count; index++)
            {
                var serviceType = serviceTypes[index];
                if (serviceType.IsGenericType && !serviceType.IsGenericTypeDefinition &&
                    serviceType.FullName == null)
                {
                    serviceTypes[index] = serviceType.GetGenericTypeDefinition();
                }
            }

            return serviceTypes.ToArray();
        }

        private ServiceLifetime CastToMsLifeTime(LifetimeStyle lifeTime)
        {
            if (lifeTime == LifetimeStyle.Transient) return ServiceLifetime.Transient;
            if (lifeTime == LifetimeStyle.Singleton) return ServiceLifetime.Singleton;

            return ServiceLifetime.Scoped;
        }
    }
}
