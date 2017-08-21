using System;

namespace Naruto.Extensions.Dependency
{
    public static class ServiceProviderServiceExtensions
    {
        public static T GetService<T>(this IServiceProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            return (T)provider.GetService(typeof(T));
        }
    }
}
