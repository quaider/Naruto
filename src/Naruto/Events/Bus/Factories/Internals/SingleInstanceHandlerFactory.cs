using Naruto.Events.Bus.Handlers;

namespace Naruto.Events.Bus.Factories.Internals
{
    /// <summary>
    /// 提供单个处理程序实例的事件处理程序工厂(维护事件处理程序的一个实例)
    /// </summary>
    /// <remarks>
    internal class SingleInstanceHandlerFactory : IEventHandlerFactory
    {
        /// <summary>
        /// The event handler instance.
        /// </summary>
        public IEventHandler HandlerInstance { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        public SingleInstanceHandlerFactory(IEventHandler handler)
        {
            HandlerInstance = handler;
        }

        public IEventHandler GetHandler()
        {
            return HandlerInstance;
        }

        public void ReleaseHandler(IEventHandler handler)
        {

        }
    }
}
