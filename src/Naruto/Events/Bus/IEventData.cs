using System;

namespace Naruto.Events.Bus
{
    /// <summary>
    ///  定义事件源公共接口
    /// </summary>
    public interface IEventData
    {
        /// <summary>
        /// 事件发生后的时间
        /// </summary>
        DateTime EventTime { get; set; }

        /// <summary>
        /// 触发事件的对象
        /// </summary>
        object EventSource { get; set; }
    }
}
