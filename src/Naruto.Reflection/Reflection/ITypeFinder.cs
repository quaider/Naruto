using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Naruto.Reflection
{
    /// <summary>
    /// 类型查找器
    /// </summary>
    public interface ITypeFinder
    {
        IList<Assembly> GetAssemblies();

        IEnumerable<Type> OfType(Type assignTypeFrom, bool onlyConcreteClasses = true);

        IEnumerable<Type> OfType<T>(bool onlyConcreteClasses = true);

        IEnumerable<Type> OfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

        IEnumerable<Type> OfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

        IEnumerable<Type> OfType(Type assignTypeFrom, Assembly assembly, bool onlyConcreteClasses = true);

        IEnumerable<Type> OfType<T>(Assembly assembly, bool onlyConcreteClasses = true);
    }
}
