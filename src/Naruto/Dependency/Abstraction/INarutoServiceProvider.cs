using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Naruto.Dependency.Abstraction
{
    public interface INarutoServiceProvider
    {
        IServiceCollection Services { get; }

        IConfigurationRoot Configuration { get; }

        IServiceProvider Build();
    }
}
