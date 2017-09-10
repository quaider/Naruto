using System;

namespace Naruto.Events.Bus
{
    /// <summary>
    /// 提供 <see cref="IEventData"/> 的基础实现
    /// </summary>
    [Serializable]
    public abstract class EventData : IEventData
    {
        public DateTime EventTime { get; set; }

        public object EventSource { get; set; }

        protected EventData()
        {
            EventTime = DateTime.Now;
        }
    }
}
