namespace Naruto.Dependency.Abstraction
{
    /// <summary>
    /// 定义依赖注入构建器，解析依赖注入服务映射信息进行构建
    /// 应用程序可实现此接口完成自定义注册
    /// </summary>
    public interface IIocBuilder
    {
        /// <summary>
        /// 开始构建依赖注入映射
        /// </summary>
        /// <returns>服务提供者</returns>
        void Build();
    }
}
