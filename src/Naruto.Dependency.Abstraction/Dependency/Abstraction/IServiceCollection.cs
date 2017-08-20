using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Dependency.Abstraction
{
    /// <summary>
    /// 一组 <see cref="ServiceDescriptor"/> 服务描述对象的集合
    /// </summary>
    public interface IServiceCollection : IList<ServiceDescriptor>
    {
    }
}
