using Naruto.Dependency.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Naruto.Dependency
{
    public class AutofacBuilderBase : IocBuilderBase
    {
        public AutofacBuilderBase(IIocManager iocManager) : base(iocManager)
        {
        }

        protected override IServiceProvider Build(IIocManager iocManager, params Assembly[] assemblies)
        {
            return null;
        }

        private static void RegisterInternal(IServiceCollection collection)
        {

        }
    }
}
