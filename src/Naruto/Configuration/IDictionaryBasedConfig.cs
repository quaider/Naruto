using System;

namespace Naruto.Configuration
{
    /// <summary>
    /// 存储于字典的配置信息
    /// </summary>
    public interface IDictionaryBasedConfig
    {
        /// <summary>
        /// 设定配置项(存在则覆盖)
        /// </summary>
        /// <typeparam name="T">配置项的类型</typeparam>
        /// <param name="name">配置项的key值</param>
        /// <param name="value">配置项的value值</param>
        void Set<T>(string name, T value);

        /// <summary>
        /// 获取指定名称的配置
        /// </summary>
        /// <param name="name">配置项的key值</param>
        object Get(string name);

        /// <summary>
        /// 获取指定名称的配置
        /// </summary>
        /// <typeparam name="T">配置项的类型</typeparam>
        /// <param name="name">配置项的key值</param>
        T Get<T>(string name);

        /// <summary>
        /// 获取指定名称的配置(无则返回设定的默认值)
        /// </summary>
        /// <param name="name">配置项的key值</param>
        /// <param name="defaultValue">如果没有找到指定name的配置，则返回该项</param>
        object Get(string name, object defaultValue);

        /// <summary>
        /// 获取指定名称的配置(无则返回设定的默认值)
        /// </summary>
        /// <typeparam name="T">配置项的类型</typeparam>
        /// <param name="name">配置项的key值</param>
        /// <param name="defaultValue">如果没有找到指定name的配置，则返回该项</param>
        /// <returns></returns>
        T Get<T>(string name, T defaultValue);

        /// <summary>
        /// 获取指定名称的配置(没有则创建)
        /// </summary>
        /// <typeparam name="T">配置项的类型</typeparam>
        /// <param name="name">配置项的key值</param>
        /// <param name="creator">如果没有找到指定name的配置，则使用该工厂创建</param>
        T GetOrCreate<T>(string name, Func<T> creator);
    }
}
