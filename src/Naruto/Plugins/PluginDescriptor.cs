using System;
using System.IO;
using System.Reflection;

namespace Naruto.Plugins
{
    /// <summary>
    /// plugin描述
    /// </summary>
    public class PluginDescriptor
    {
        public PluginDescriptor(Assembly referencedAssembly, FileInfo originalAssemblyFile, Type pluginType)
        {
            PluginType = pluginType;
            ReferencedAssembly = referencedAssembly;
            OriginalAssemblyFile = originalAssemblyFile;
        }

        /// <summary>
        /// 插件中实现了IPlugin或BasePlugin的类型
        /// </summary>
        public Type PluginType { get; set; }

        /// <summary>
        /// 当前插件所引用的程序集
        /// </summary>
        public Assembly ReferencedAssembly { get; set; }

        /// <summary>
        /// 当前插件的程序集文件信息
        /// </summary>
        public FileInfo OriginalAssemblyFile { get; set; }

        public bool Installed { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 系统名称，用于程序识别
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as PluginDescriptor;

            return other != null && string.IsNullOrWhiteSpace(SystemName) && SystemName == other.SystemName;
        }

        public override int GetHashCode()
        {
            return SystemName.GetHashCode();
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
