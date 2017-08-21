using System;
using System.Collections.Generic;
using Naruto.Dependency.Abstraction;

namespace Naruto.Dependency
{
    public class IocManager : IIocManager
    {
        public static IocManager Instance { get; private set; }

        private IServiceCollection _serviceCollection;

        /// <summary>
        /// 其初始化推迟到应用层
        /// </summary>
        private IIocResolver _iocResolver;

        static IocManager()
        {
            Instance = new IocManager();
        }

        private IocManager()
        {
            _serviceCollection = new ServiceCollection();
        }

        public T Resolve<T>()
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type type)
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>(Type type)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }

        public bool IsRegistered(Type type)
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered<T>()
        {
            throw new NotImplementedException();
        }
    }
}
