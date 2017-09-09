using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Naruto.Dependency.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IContainer Populate(this IServiceCollection service, Action<ContainerBuilder> populate)
        {
            populate(IocManager.Instance.ContainerBuilder);

            return IocManager.Instance.Build();
        }
    }
}
