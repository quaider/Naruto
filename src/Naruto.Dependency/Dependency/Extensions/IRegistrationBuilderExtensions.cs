using Autofac.Builder;
using Naruto.Dependency.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Dependency.Extensions
{
    internal static class IRegistrationBuilderExtensions
    {
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> AsLifeTime<TLimit, TActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registrar,
            LifetimeStyle lifetime)
        {
            if (lifetime == LifetimeStyle.Singleton)
                return registrar.SingleInstance();
            if (lifetime == LifetimeStyle.Scoped)
                return registrar.InstancePerLifetimeScope();

            return registrar;
        }
    }
}
