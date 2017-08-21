using System;
using System.Diagnostics;
using System.Text;

namespace Naruto.Dependency.Abstraction
{
    /// <summary>
    /// 依赖注入组件描述信息(包含注册组件的服务类型、组件类型、生命周期等描述信息,以便IOC根据这些信息自动注册)
    /// </summary>
    public class ServiceDescriptor
    {
        #region Constructor

        /// <summary>
        /// 使用指定服务类型，实现类型，生命周期创建 ServiceDescriptor 实例
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <param name="implementationType">实现类型</param>
        /// <param name="lifetime">生命周期</param>
        public ServiceDescriptor(Type serviceType, Type implementationType, LifetimeStyle lifetime) : this(serviceType, lifetime)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
        }

        /// <summary>
        /// 使用指定实例初始化ServiceDescriptor，其生命周期为 <see cref="LifetimeStyle.Singleton"/>
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <param name="instance">服务类型对应单例实例</param>
        public ServiceDescriptor(Type serviceType, object instance) : this(serviceType, LifetimeStyle.Singleton)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            ImplementationInstance = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        public ServiceDescriptor(Type serviceType, Func<IServiceProvider, object> factory, LifetimeStyle lifetime) : this(serviceType, lifetime)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            ImplementationFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        private ServiceDescriptor(Type serviceType, LifetimeStyle lifetime)
        {
            Lifetime = lifetime;
            ServiceType = serviceType;
        }

        #endregion

        #region 相等性检查(组件的接口类型和本身实例的类型都相等，则认为具有相等性)

        /// <summary>
        /// 组件类型和服务类型均相等则认为对象一样
        /// </summary>
        /// <param name="obj">要比较的对象实例</param>
        /// <returns>是否相等的布尔值</returns>
        public override bool Equals(object obj)
        {
            var typedObj = obj as ServiceDescriptor;
            if (typedObj == null) return false;

            return typedObj.ServiceType == ServiceType && typedObj.ImplementationType == ImplementationType;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return ServiceType.GetHashCode() + ImplementationType.GetHashCode();
        }

        #endregion

        #region 公有属性

        /// <summary>
        /// 生命周期
        /// </summary>
        public LifetimeStyle Lifetime { get; }

        /// <summary>
        /// 服务类型
        /// </summary>
        public Type ServiceType { get; }

        /// <summary>
        /// 实现类型
        /// </summary>
        public Type ImplementationType { get; }

        /// <summary>
        /// 服务类型的实例
        /// </summary>
        public object ImplementationInstance { get; }

        /// <summary>
        /// 服务类型的实例 <see cref="ImplementationInstance"/> 的创建工厂
        /// </summary>
        public Func<IServiceProvider, object> ImplementationFactory { get; }

        #endregion

        #region 方法

        internal Type GetImplementationType()
        {
            if (ImplementationType != null)
            {
                return ImplementationType;
            }
            else if (ImplementationInstance != null)
            {
                return ImplementationInstance.GetType();
            }
            else if (ImplementationFactory != null)
            {
                var typeArguments = ImplementationFactory.GetType().GenericTypeArguments;

                Debug.Assert(typeArguments.Length == 2);

                return typeArguments[1];
            }

            Debug.Assert(false, "ImplementationType, ImplementationInstance or ImplementationFactory must be non null");
            return null;
        }

        private static ServiceDescriptor Describe<TService, TImplementation>(LifetimeStyle lifetime)
            where TService : class
            where TImplementation : class, TService
        {
            return Describe(typeof(TService), typeof(TImplementation), lifetime: lifetime);
        }

        public static ServiceDescriptor Describe(Type serviceType, Type implementationType, LifetimeStyle lifetime)
        {
            return new ServiceDescriptor(serviceType, implementationType, lifetime);
        }

        public static ServiceDescriptor Describe(Type serviceType, Func<IServiceProvider, object> implementationFactory, LifetimeStyle lifetime)
        {
            return new ServiceDescriptor(serviceType, implementationFactory, lifetime);
        }

        #endregion
    }
}
