using System;

namespace Naruto.Dependency.Abstraction
{
    public class DefaultServiceProvider : IServiceProvider
    {
        private IIocResolver _resolver;

        public DefaultServiceProvider(IIocResolver resolver)
        {
            _resolver = resolver;
        }

        public object GetService(Type serviceType)
        {
            return _resolver.Resolve(serviceType);
        }
    }
}
