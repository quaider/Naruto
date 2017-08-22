using System;

namespace Naruto.Dependency.Abstraction
{
    public interface IIocManager : IIocRegistrar, IIocResolver, IDisposable
    {
    }
}
