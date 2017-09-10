
namespace Naruto.Events.Bus.Handlers
{
    /// <summary>
    /// 定义一个处理<see cref="TEventData"/>类型的事件处理器
    /// </summary>
    /// <typeparam name="TEventData">事件源类型</typeparam>
    public interface IEventHandler<in TEventData> : IEventHandler
    {
        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="eventData">事件源</param>
        void HandleEvent(TEventData eventData);
    }
}
