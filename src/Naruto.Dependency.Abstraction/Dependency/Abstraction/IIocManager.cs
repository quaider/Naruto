using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Dependency.Abstraction
{
    public interface IIocManager : IIocResolver, IDisposable
    {
    }
}
