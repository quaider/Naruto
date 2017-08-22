using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Naruto.Reflection
{
    public class AppDomainTypeFinder : ITypeFinder
    {
        #region 字段

        private readonly bool _ignoreReflectionErrors = true;

        /// <summary>
        /// 是否加载程序域中的程序集
        /// </summary>
        private bool _loadAppDomainAssemblies = true;

        /// <summary>
        /// 需要忽略加载的程序集
        /// </summary>
        private string _assemblySkipLoadingPattern =
            "^System|^mscorlib|^Microsoft|^AjaxControlToolkit|^Antlr3|^Autofac|^AutoMapper|^Castle|^ComponentArt|^CppCodeProvider|^DotNetOpenAuth|^EntityFramework|^EPPlus|^FluentValidation|^ImageResizer|^itextsharp|^log4net|^MaxMind|^MbUnit|^MiniProfiler|^Mono.Math|^MvcContrib|^Newtonsoft|^NHibernate|^nunit|^Org.Mentalis|^PerlRegex|^QuickGraph|^Recaptcha|^Remotion|^RestSharp|^Rhino|^Telerik|^Iesi|^TestDriven|^TestFu|^UserAgentStringLibrary|^VJSharpCodeProvider|^WebActivator|^WebDev|^WebGrease";

        /// <summary>
        /// 限制加载的程序集
        /// </summary>
        private string _assemblyRestrictToLoadingPattern = ".*";

        #endregion

        #region 属性

        /// <summary>
        /// 当前应用程序域
        /// </summary>
        public virtual AppDomain App
        {
            get { return AppDomain.CurrentDomain; }
        }

        /// <summary>
        /// 是否加载程序域中的程序集
        /// </summary>
        public bool LoadAppDomainAssemblies
        {
            get { return _loadAppDomainAssemblies; }
            set { _loadAppDomainAssemblies = value; }
        }

        /// <summary>
        /// 指定需要额外加载的程序集名列表
        /// </summary>
        public IList<string> AssemblyNames { get; set; } = new List<string>();

        /// <summary>
        /// 已加载的程序集
        /// </summary>
        protected IList<Assembly> LoadedAssemblies { get; set; } = new List<Assembly>();

        /// <summary>
        /// 忽略加载的程序集正则表达式
        /// </summary>
        public string AssemblySkipLoadingPattern
        {
            get { return _assemblySkipLoadingPattern; }
            set { _assemblySkipLoadingPattern = value; }
        }

        /// <summary>
        /// 限制加载的程序集正则表达式
        /// </summary>
        public string AssemblyRestrictToLoadingPattern
        {
            get { return _assemblyRestrictToLoadingPattern; }
            set { _assemblyRestrictToLoadingPattern = value; }
        }

        #endregion

        #region Methods

        public virtual IList<Assembly> GetAssemblies()
        {
            //加载程序域中的程序集
            if (LoadAppDomainAssemblies)
                GetAssembliesInAppDomain();

            //需额外加载的程序集
            GetConfiguredAssemblies();

            return LoadedAssemblies;
        }

        public IEnumerable<Type> OfType(Type assignTypeFrom, bool onlyConcreteClasses = true)
        {
            return OfType(assignTypeFrom, GetAssemblies(), onlyConcreteClasses);
        }

        public IEnumerable<Type> OfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies,
            bool onlyConcreteClasses = true)
        {
            var result = new List<Type>();
            try
            {
                foreach (var a in assemblies)
                {
                    Type[] types = null;
                    try
                    {
                        types = a.GetTypes();
                    }
                    catch
                    {
                        //是否忽略加载错误
                        if (!_ignoreReflectionErrors)
                        {
                            throw;
                        }
                    }

                    if (types != null)
                    {
                        foreach (var t in types)
                        {
                            if (assignTypeFrom.IsAssignableFrom(t) ||
                                (assignTypeFrom.IsGenericTypeDefinition &&
                                 DoesTypeImplementOpenGeneric(t, assignTypeFrom)))
                            {
                                if (!t.IsInterface)
                                {
                                    if (onlyConcreteClasses)
                                    {
                                        if (t.IsClass && !t.IsAbstract)
                                        {
                                            result.Add(t);
                                        }
                                    }
                                    else
                                    {
                                        result.Add(t);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                var msg = string.Empty;
                foreach (var e in ex.LoaderExceptions)
                    msg += e.Message + Environment.NewLine;

                var fail = new Exception(msg, ex);
                Debug.WriteLine(fail.Message, fail);

                throw fail;
            }

            return result;
        }

        public IEnumerable<Type> OfType<T>(bool onlyConcreteClasses = true)
        {
            return OfType(typeof(T), onlyConcreteClasses);
        }

        public IEnumerable<Type> OfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
        {
            return OfType(typeof(T), assemblies, onlyConcreteClasses);
        }

        public IEnumerable<Type> OfType(Type assignTypeFrom, Assembly assembly, bool onlyConcreteClasses = true)
        {
            return OfType(assignTypeFrom, new[] { assembly }, onlyConcreteClasses);
        }

        public IEnumerable<Type> OfType<T>(Assembly assembly, bool onlyConcreteClasses = true)
        {
            return OfType(typeof(T), assembly, onlyConcreteClasses);
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// Does type implement generic?
        /// </summary>
        /// <param name="type"></param>
        /// <param name="openGeneric"></param>
        /// <returns></returns>
        protected virtual bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric)
        {
            try
            {
                var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();
                foreach (var implementedInterface in type.FindInterfaces((objType, objCriteria) => true, null))
                {
                    if (!implementedInterface.IsGenericType) continue;

                    var isMatch = genericTypeDefinition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition());
                    return isMatch;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 验证程序集是否符合给定的模式
        /// </summary>
        /// <param name="assemblyFullName">程序集完全限定名</param>
        /// <param name="pattern">匹配模式</param>
        /// <returns></returns>
        protected virtual bool Matches(string assemblyFullName, string pattern)
        {
            return Regex.IsMatch(assemblyFullName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyFullName">程序集完全限定名</param>
        public virtual bool Matches(string assemblyFullName)
        {
            return !Matches(assemblyFullName, AssemblySkipLoadingPattern)
                   && Matches(assemblyFullName, AssemblyRestrictToLoadingPattern);
        }

        /// <summary>
        /// 加载并返回指定的程序集
        /// </summary>
        /// <param name="addedAssemblyNames"></param>
        /// <param name="assemblies"></param>
        protected void GetConfiguredAssemblies()
        {
            foreach (var assemblyName in AssemblyNames)
            {
                var assembly = Assembly.Load(assemblyName);
                if (!LoadedAssemblies.Contains(assembly))
                {
                    LoadedAssemblies.Add(assembly);
                }
            }
        }

        private void GetAssembliesInAppDomain()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (Matches(assembly.FullName) && !LoadedAssemblies.Contains(assembly))
                {
                    LoadedAssemblies.Add(assembly);
                }
            }
        }

        #endregion
    }
}
