using System;
using System.Threading.Tasks;
using Naruto.Events.Bus.Factories;
using Naruto.Events.Bus.Handlers;

namespace Naruto.Events.Bus
{
    public interface IEventBus
    {
        #region Register

        /// <summary>
        /// 注册单例事件处理程序实例到容器
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <param name="action">事件处理程序委托(内部转换为单例处理程序工厂，全局维护一个实例)</param>
        IDisposable Register<TEventData>(Action<TEventData> action) where TEventData : IEventData;

        /// <summary>
        /// 注册单例事件处理程序实例到容器
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <param name="handler">事件处理程序(内部转换为单例处理程序工厂，全局维护一个实例)</param>
        IDisposable Register<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData;

        /// <summary>
        /// 注册瞬时事件处理程序实例到容器
        /// 出发事件时，每次都会创建 <see cref="THandler"/> 的一个实例
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <typeparam name="THandler">事件处理程序类型，必须具有默认构造函数</typeparam>
        IDisposable Register<TEventData, THandler>() where TEventData : IEventData where THandler : IEventHandler<TEventData>, new();

        /// <summary>
        /// 注册单例事件处理程序实例到容器
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handler">事件处理程序(内部转换为单例处理程序工厂，全局维护一个实例)</param>
        IDisposable Register(Type eventType, IEventHandler handler);

        /// <summary>
        /// 注册指定类型的事件到事件总线(使用指定处理程序工厂)
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <param name="handlerFactory">处理程序工厂</param>
        IDisposable Register<TEventData>(IEventHandlerFactory handlerFactory) where TEventData : IEventData;

        /// <summary>
        /// 注册指定类型的事件到事件总线(使用指定处理程序工厂)
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handlerFactory">处理程序工厂</param>
        IDisposable Register(Type eventType, IEventHandlerFactory handlerFactory);

        #endregion

        #region Unregister

        /// <summary>
        /// Unregisters from an event.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="action"></param>
        void Unregister<TEventData>(Action<TEventData> action) where TEventData : IEventData;

        /// <summary>
        /// Unregisters from an event.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="handler">Handler object that is registered before</param>
        void Unregister<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData;

        /// <summary>
        /// Unregisters from an event.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Handler object that is registered before</param>
        void Unregister(Type eventType, IEventHandler handler);

        /// <summary>
        /// Unregisters from an event.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="factory">Factory object that is registered before</param>
        void Unregister<TEventData>(IEventHandlerFactory factory) where TEventData : IEventData;

        /// <summary>
        /// Unregisters from an event.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="factory">Factory object that is registered before</param>
        void Unregister(Type eventType, IEventHandlerFactory factory);

        /// <summary>
        /// Unregisters all event handlers of given event type.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        void UnregisterAll<TEventData>() where TEventData : IEventData;

        /// <summary>
        /// Unregisters all event handlers of given event type.
        /// </summary>
        /// <param name="eventType">Event type</param>
        void UnregisterAll(Type eventType);

        #endregion

        #region Trigger

        /// <summary>
        /// Triggers an event.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="eventData">Related data for the event</param>
        void Trigger<TEventData>(TEventData eventData) where TEventData : IEventData;

        /// <summary>
        /// Triggers an event.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="eventSource">The object which triggers the event</param>
        /// <param name="eventData">Related data for the event</param>
        void Trigger<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData;

        /// <summary>
        /// Triggers an event.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="eventData">Related data for the event</param>
        void Trigger(Type eventType, IEventData eventData);

        /// <summary>
        /// Triggers an event.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="eventSource">The object which triggers the event</param>
        /// <param name="eventData">Related data for the event</param>
        void Trigger(Type eventType, object eventSource, IEventData eventData);

        /// <summary>
        /// Triggers an event asynchronously.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="eventData">Related data for the event</param>
        /// <returns>The task to handle async operation</returns>
        Task TriggerAsync<TEventData>(TEventData eventData) where TEventData : IEventData;

        /// <summary>
        /// Triggers an event asynchronously.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="eventSource">The object which triggers the event</param>
        /// <param name="eventData">Related data for the event</param>
        /// <returns>The task to handle async operation</returns>
        Task TriggerAsync<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData;

        /// <summary>
        /// Triggers an event asynchronously.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="eventData">Related data for the event</param>
        /// <returns>The task to handle async operation</returns>
        Task TriggerAsync(Type eventType, IEventData eventData);

        /// <summary>
        /// Triggers an event asynchronously.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="eventSource">The object which triggers the event</param>
        /// <param name="eventData">Related data for the event</param>
        /// <returns>The task to handle async operation</returns>
        Task TriggerAsync(Type eventType, object eventSource, IEventData eventData);


        #endregion
    }
}
