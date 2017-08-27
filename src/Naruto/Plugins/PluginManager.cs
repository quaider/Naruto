using System;
using System.IO;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using Naruto.Resources;
using System.Diagnostics;

namespace Naruto.Plugins
{
    /// <summary>
    /// 提供插件管理，引用加载等
    /// http://shazwazza.com/post/Developing-a-plugin-framework-in-ASPNET-with-medium-trust.aspx
    /// </summary>
    public class PluginManager : IPluginManager
    {
        /// <summary>
        /// 插件存放目录
        /// </summary>
        private const string PluginPath = "~/plugins";

        /// <summary>
        /// 插件dll引用目录(AppDomain从此处bin中加载程序集)
        /// </summary>
        private const string PluginPathCopy = "~/plugins/bin";

        private readonly ReaderWriterLockSlim _resourceLock = new ReaderWriterLockSlim();

        /// <summary>
        /// bin && Environment.CurrentDirectory dll file names
        /// </summary>
        private readonly List<string> _baseLibs;

        public PluginManager()
        {
            //AppDomain.CurrentDomain.BaseDirectory -> get the base directory that the assembly resolver uses to probe for assemblies
            var domainBaseDir = AppDomain.CurrentDomain.BaseDirectory;

            //获取bin/{version}中的程序集， 如 bin/debug/netcoreapp2.0/
            _baseLibs = GetAllAssemblyNamesFromPath(domainBaseDir).ToList();

            //Environment.CurrentDirectory 当前应用程序的根目录(貌似这里通常不会用dll存在)
            if (!domainBaseDir.Equals(Environment.CurrentDirectory, StringComparison.InvariantCultureIgnoreCase))
                _baseLibs.AddRange(GetAllAssemblyNamesFromPath(Environment.CurrentDirectory));

        }

        /// <summary>
        /// 初始化
        /// 复制 ~/plugins目录下的文件到 ~/plugins/bin目录下
        /// 初始化插件安装记录
        /// 装在插件描述文件
        /// 加载程序集
        /// </summary>
        public void Initialize()
        {
            using (new ResourceWriteLock(_resourceLock))
            {
                var pluginDir = new DirectoryInfo(PluginPath);
                var pluginCopyDir = new DirectoryInfo(PluginPathCopy);

                //确保 plugin 和 plugin copy 文件夹已创建
                Directory.CreateDirectory(pluginDir.FullName);
                Directory.CreateDirectory(pluginCopyDir.FullName);

                //删除plugin/bin下的所有文件，后续从plugin中copy
                DeleteDirectory(pluginCopyDir);

                //加载插件的描述文件

            }
        }

        private void DeleteDirectory(DirectoryInfo dir)
        {
            var files = dir.GetFiles("*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                Debug.WriteLine("删除文件 " + file.Name);

                try
                {
                    file.Delete();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"文件{file.Name}删除失败，失败原因：{ex}");
                }
            }
        }

        /// <summary>
        /// 查找指定路径下的dll并返回其文件短名称(默认只搜索顶层目录)
        /// </summary>
        private IEnumerable<string> GetAllAssemblyNamesFromPath(string path, SearchOption searchOptions = SearchOption.TopDirectoryOnly)
        {
            return new DirectoryInfo(path).GetFiles("*.dll", searchOptions)
                                          .Select(f => f.Name);
        }
    }
}
