using Naruto.Dependency.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Naruto.Extensions.Dependency
{
    public static class ServiceCollectionDescriptorExtensions
    {
        public static IServiceCollection Add(this IServiceCollection collection, ServiceDescriptor descriptor)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            collection.Add(descriptor);

            return collection;
        }

        public static IServiceCollection Add(this IServiceCollection collection, IEnumerable<ServiceDescriptor> descriptors)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (descriptors == null)
            {
                throw new ArgumentNullException(nameof(descriptors));
            }

            foreach (var descriptor in descriptors)
            {
                collection.Add(descriptor);
            }

            return collection;
        }

        public static void TryAdd(this IServiceCollection collection, ServiceDescriptor descriptor)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            if (!collection.Any(d => d.Equals(descriptor)))
            {
                collection.Add(descriptor);
            }
        }

        public static void TryAdd(this IServiceCollection collection, IEnumerable<ServiceDescriptor> descriptors)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (descriptors == null)
            {
                throw new ArgumentNullException(nameof(descriptors));
            }

            foreach (var d in descriptors)
            {
                collection.TryAdd(d);
            }
        }

        #region Add Transient

        public static void TryAddTransient(this IServiceCollection collection, Type serviceType)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            var descriptor = ServiceDescriptor.Describe(serviceType, serviceType, LifetimeStyle.Transient);

            TryAdd(collection, descriptor);
        }

        public static void TryAddTransient(this IServiceCollection collection, Type serviceType, Type implementationType)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (implementationType == null)
            {
                throw new ArgumentNullException(nameof(implementationType));
            }

            var descriptor = ServiceDescriptor.Describe(serviceType, implementationType, LifetimeStyle.Transient);

            TryAdd(collection, descriptor);
        }

        public static void TryAddTransient(this IServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (implementationFactory == null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            var descriptor = ServiceDescriptor.Describe(service, implementationFactory, LifetimeStyle.Transient);

            TryAdd(collection, descriptor);
        }

        public static void TryAddTransient<TService>(this IServiceCollection collection)
            where TService : class
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            TryAddTransient(collection, typeof(TService), typeof(TService));
        }

        public static void TryAddTransient<TService, TImplementation>(this IServiceCollection collection)
            where TService : class
            where TImplementation : class, TService
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            TryAddTransient(collection, typeof(TService), typeof(TImplementation));
        }

        public static void TryAddTransient<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            TryAddTransient(services, implementationFactory);
        }

        #endregion

        #region Add Scoped

        public static void TryAddScoped(this IServiceCollection collection, Type service)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            var descriptor = ServiceDescriptor.Describe(service, service, LifetimeStyle.Scoped);

            TryAdd(collection, descriptor);
        }

        public static void TryAddScoped(this IServiceCollection collection, Type service, Type implementationType)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (implementationType == null)
            {
                throw new ArgumentNullException(nameof(implementationType));
            }

            var descriptor = ServiceDescriptor.Describe(service, implementationType, LifetimeStyle.Scoped);

            TryAdd(collection, descriptor);
        }

        public static void TryAddScoped(this IServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (implementationFactory == null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            var descriptor = ServiceDescriptor.Describe(service, implementationFactory, LifetimeStyle.Scoped);

            TryAdd(collection, descriptor);
        }

        public static void TryAddScoped<TService>(this IServiceCollection collection)
            where TService : class
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            TryAddScoped(collection, typeof(TService), typeof(TService));
        }

        public static void TryAddScoped<TService, TImplementation>(this IServiceCollection collection)
            where TService : class
            where TImplementation : class, TService
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            TryAddScoped(collection, typeof(TService), typeof(TImplementation));
        }

        public static void TryAddScoped<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            TryAddScoped(services, implementationFactory);
        }

        #endregion

        #region Add Singleton

        public static void TryAddSingleton(this IServiceCollection collection, Type service)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            var descriptor = ServiceDescriptor.Describe(service, service, LifetimeStyle.Singleton);

            TryAdd(collection, descriptor);
        }

        public static void TryAddSingleton(this IServiceCollection collection, Type service, Type implementationType)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (implementationType == null)
            {
                throw new ArgumentNullException(nameof(implementationType));
            }

            var descriptor = ServiceDescriptor.Describe(service, implementationType, LifetimeStyle.Singleton);

            TryAdd(collection, descriptor);
        }

        public static void TryAddSingleton(this IServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (implementationFactory == null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            var descriptor = ServiceDescriptor.Describe(service, implementationFactory, LifetimeStyle.Singleton);

            TryAdd(collection, descriptor);
        }

        public static void TryAddSingleton<TService>(this IServiceCollection collection)
            where TService : class
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            TryAddSingleton(collection, typeof(TService), typeof(TService));
        }

        public static void TryAddSingleton<TService, TImplementation>(this IServiceCollection collection)
            where TService : class
            where TImplementation : class, TService
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            TryAddSingleton(collection, typeof(TService), typeof(TImplementation));
        }

        public static void TryAddSingleton<TService>(this IServiceCollection collection, TService instance)
            where TService : class
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var descriptor = new ServiceDescriptor(typeof(TService), instance);

            TryAdd(collection, descriptor);
        }

        public static void TryAddSingleton<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            var descriptor = new ServiceDescriptor(typeof(TService), implementationFactory, LifetimeStyle.Singleton);
            services.TryAdd(descriptor);
        }

        #endregion

        #region Replace

        public static IServiceCollection Replace(this IServiceCollection collection, ServiceDescriptor descriptor)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            var registeredServiceDescriptor = collection.FirstOrDefault(s => s.ServiceType == descriptor.ServiceType);
            if (registeredServiceDescriptor != null)
            {
                collection.Remove(registeredServiceDescriptor);
            }

            collection.Add(descriptor);

            return collection;
        }

        #endregion

        #region Remove All

        public static IServiceCollection RemoveAll<T>(this IServiceCollection collection)
        {
            return RemoveAll(collection, typeof(T));
        }

        public static IServiceCollection RemoveAll(this IServiceCollection collection, Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            for (var i = collection.Count - 1; i >= 0; i--)
            {
                var descriptor = collection[i];
                if (descriptor.ServiceType == serviceType)
                {
                    collection.RemoveAt(i);
                }
            }

            return collection;
        }

        #endregion
    }
}
