using System;
namespace Naruto.Plugins
{
    /// <summary>
    /// 插件必须实现的接口
    /// </summary>
    public interface IPlugin
    {
        PluginDescriptor PluginDescriptor { get; set; }

        void Install();

        void Uninstall();
    }
}
