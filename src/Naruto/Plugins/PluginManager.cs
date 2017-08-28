using System;
using System.IO;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using Naruto.Resources;
using System.Diagnostics;
using System.Reflection;
using Naruto.Reflection;

namespace Naruto.Plugins
{
    /// <summary>
    /// 提供插件管理，引用加载等 from nopcommerce
    /// http://shazwazza.com/post/Developing-a-plugin-framework-in-ASPNET-with-medium-trust.aspx
    /// </summary>
    internal class PluginManager
    {
        /// <summary>
        /// 已安装插件记录
        /// </summary>
        private const string InstalledPluginsFilePath = "~/App_Data/InstalledPlugins.txt";

        /// <summary>
        /// 插件存放目录
        /// </summary>
        private const string PluginPath = "~/plugins";

        /// <summary>
        /// 插件dll引用目录(AppDomain从此处bin中加载程序集)
        /// </summary>
        private const string PluginPathCopy = @"~/plugins/bin";

        private readonly ReaderWriterLockSlim _resourceLock = new ReaderWriterLockSlim();

        /// <summary>
        /// bin && Environment.CurrentDirectory dll file names
        /// </summary>
        private readonly List<string> _baseLibs;

        private ITypeFinder _finder;

        public static PluginManager Instance => new PluginManager();

        private PluginManager()
        {
            _finder = AppDomainTypeFinder.Instance;

            //AppDomain.CurrentDomain.BaseDirectory -> get the base directory that the assembly resolver uses to probe for assemblies
            var domainBaseDir = AppDomain.CurrentDomain.BaseDirectory;

            //获取bin/{version}中的程序集， 如 bin/debug/netcoreapp2.0/
            _baseLibs = GetAllAssemblyNamesFromPath(domainBaseDir).ToList();

            //Environment.CurrentDirectory 当前应用程序的根目录(貌似这里通常不会有dll存在)
            if (!domainBaseDir.Equals(Environment.CurrentDirectory, StringComparison.InvariantCultureIgnoreCase))
                _baseLibs.AddRange(GetAllAssemblyNamesFromPath(Environment.CurrentDirectory));
        }

        public IEnumerable<PluginDescriptor> ReferencedPlugins { get; set; }

        /// <summary>
        /// 初始化插件
        /// 复制 ~/plugins目录下的文件到 ~/plugins/bin目录下
        /// 初始化插件安装记录
        /// 装在插件描述文件
        /// 加载程序集
        /// </summary>
        internal void Initialize()
        {
            using (new ResourceWriteLock(_resourceLock))
            {
                var pluginDir = new DirectoryInfo(Constant.NarutoPath.MapPath(PluginPath));
                var pluginCopyDir = new DirectoryInfo(Constant.NarutoPath.MapPath(PluginPathCopy));

                //确保 plugin 和 plugin copy 文件夹已创建
                Directory.CreateDirectory(pluginDir.FullName);
                Directory.CreateDirectory(pluginCopyDir.FullName);

                //删除plugin/bin下的所有文件，后续从plugin中copy
                var pluginBinFiles = DeleteDirectory(pluginCopyDir);

                var referencedPlugins = new List<PluginDescriptor>();

                var installedPluginSysNames = PluginFileParser.ParseInstalledPluginsFile(Constant.NarutoPath.MapPath(InstalledPluginsFilePath));

                //加载插件的描述文件
                foreach (var item in LoadPluginDescriptors(pluginDir))
                {
                    if (string.IsNullOrWhiteSpace(item.descriptor.SystemName))
                        throw new Exception($"{item.decriptorFile.Name} has no systemt name");

                    if (referencedPlugins.Contains(item.descriptor))
                        throw new Exception($"A plugin with '{item.descriptor.SystemName}' system name is already defined");

                    item.descriptor.Installed = installedPluginSysNames.FirstOrDefault(f => f.Equals(item.descriptor.SystemName, StringComparison.InvariantCultureIgnoreCase)) != null;

                    //获取~/plugins中的所有dll(排除~/plugins/bin,确保没有重复注册)
                    var pluginDlls = item.decriptorFile.Directory.GetFiles("*.dll", SearchOption.AllDirectories)
                                            .Where(f => !pluginBinFiles.Select(x => x.FullName).Contains(f.FullName))
                                            .ToList();

                    //eg: Naruto.Plugin.xxxx.dll
                    item.descriptor.OriginalAssemblyFile = pluginDlls.FirstOrDefault(f => f.Name.Equals(item.descriptor.PluginFileName,
                        StringComparison.InvariantCultureIgnoreCase));

                    //复制该dll到 plugin/bin
                    item.descriptor.ReferencedAssembly = CopyToPluginBinFolder(item.descriptor.OriginalAssemblyFile);

                    //复制其他dll 如果 还没有加载
                    foreach (var dllFile in pluginDlls
                        .Where(f => !f.Name.Equals(item.descriptor.OriginalAssemblyFile.Name, StringComparison.InvariantCultureIgnoreCase))
                        .Where(f => !IsAlreadyLoaded(f)))
                    {
                        CopyToPluginBinFolder(dllFile);
                    }

                    //init plugin type (only one plugin per assembly is allowed)
                    item.descriptor.PluginType = _finder.OfType<IPlugin>(item.descriptor.ReferencedAssembly).FirstOrDefault();

                    referencedPlugins.Add(item.descriptor);
                }

                ReferencedPlugins = referencedPlugins;
            }
        }

        public static void MarkPluginAsInstalled(string systemName)
        {
            Guard.NotNullOrWhiteSpace(systemName, nameof(systemName));

            var filePath = Constant.NarutoPath.MapPath(InstalledPluginsFilePath);
            if (!File.Exists(filePath))
                using (File.Create(filePath))
                {
                    //we use 'using' to close the file after it's created
                }

            var installedPluginSystemNames = PluginFileParser.ParseInstalledPluginsFile(filePath);
            bool alreadyMarkedAsInstalled = installedPluginSystemNames
                                                .FirstOrDefault(x => x.Equals(systemName, StringComparison.InvariantCultureIgnoreCase)) != null;
            if (!alreadyMarkedAsInstalled)
                installedPluginSystemNames.Add(systemName);

            PluginFileParser.SaveInstalledPluginsFile(installedPluginSystemNames, filePath);
        }

        public static void MarkPluginAsUninstalled(string systemName)
        {
            Guard.NotNullOrWhiteSpace(systemName, nameof(systemName));

            var filePath = Constant.NarutoPath.MapPath(InstalledPluginsFilePath);
            if (!File.Exists(filePath))
                using (File.Create(filePath))
                {
                    //we use 'using' to close the file after it's created
                }

            var installedPluginSystemNames = PluginFileParser.ParseInstalledPluginsFile(filePath);
            bool alreadyMarkedAsInstalled = installedPluginSystemNames
                                                .FirstOrDefault(x => x.Equals(systemName, StringComparison.InvariantCultureIgnoreCase)) != null;
            if (alreadyMarkedAsInstalled)
                installedPluginSystemNames.Remove(systemName);

            PluginFileParser.SaveInstalledPluginsFile(installedPluginSystemNames, filePath);
        }

        private FileInfo[] DeleteDirectory(DirectoryInfo dir)
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

            return files;
        }

        /// <summary>
        /// 查找指定路径下的dll并返回其文件短名称(默认只搜索顶层目录)
        /// </summary>
        private IEnumerable<string> GetAllAssemblyNamesFromPath(string path, SearchOption searchOptions = SearchOption.TopDirectoryOnly)
        {
            return new DirectoryInfo(path).GetFiles("*.dll", searchOptions)
                                          .Select(f => f.Name);
        }

        private IEnumerable<(FileInfo decriptorFile, PluginDescriptor descriptor)> LoadPluginDescriptors(DirectoryInfo dir)
        {
            Guard.NotNull(dir, nameof(dir));

            var result = new List<(FileInfo originalReferencedFile, PluginDescriptor descriptor)>();

            foreach (var file in dir.GetFiles("description.json", SearchOption.AllDirectories))
            {
                var content = File.ReadAllText(file.FullName);
                var descriptor = Newtonsoft.Json.JsonConvert.DeserializeObject<PluginDescriptor>(content);

                result.Add((file, descriptor));
            }

            return result.OrderBy(f => f.descriptor.DisplayOrder);
        }

        private Assembly CopyToPluginBinFolder(FileInfo dllFile)
        {
            var pluginDir = new DirectoryInfo(Constant.NarutoPath.MapPath(PluginPath));
            var pluginCopyDir = new DirectoryInfo(Constant.NarutoPath.MapPath(PluginPathCopy));

            var destFile = dllFile.CopyTo(Path.Combine(pluginCopyDir.FullName, dllFile.Name), true);
            var assembly = Assembly.LoadFile(destFile.FullName);

            return assembly;
        }

        private bool IsAlreadyLoaded(FileInfo fileInfo)
        {
            if (_baseLibs.Any(lib => lib.Equals(fileInfo.Name, StringComparison.InvariantCultureIgnoreCase)))
                return true;

            try
            {
                var fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileInfo.FullName);
                if (string.IsNullOrEmpty(fileNameWithoutExt))
                    throw new Exception($"Cannot get file extension for {fileInfo.Name}");

                foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    var assemblyName = a.FullName.Split(',').FirstOrDefault();
                    if (fileNameWithoutExt.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine("Cannot validate whether an assembly is already loaded. " + exc);
            }

            return false;
        }
    }
}
