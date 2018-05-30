using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using GammaLibrary.Extensions;

// ReSharper disable PossibleNullReferenceException

namespace CardGameEngine.Events
{
    public class EventManager
    {
        // Object or Dynamic????
        private readonly ConcurrentDictionary<object, object> _events = new ConcurrentDictionary<object, object>();

        // 傻逼 C# 泛型
        public void RegisterEvent<TEvent, TEventArgs>(Event<TEvent, TEventArgs> identifier) where TEvent : Event<TEvent, TEventArgs>, new() =>
            _events[identifier] = new TEvent();

        public void RegisterEventHandler<TEvent, TEventArgs>(Event<TEvent, TEventArgs> identifier, EventHandler<TEventArgs> handler) where TEvent : Event<TEvent, TEventArgs>, new() =>
            ((TEvent)_events[identifier]).Register(handler);

        public void RemoveEventHandler<TEvent, TEventArgs>(Event<TEvent, TEventArgs> identifier, EventHandler<TEventArgs> handler) where TEvent : Event<TEvent, TEventArgs>, new() =>
            ((TEvent)_events[identifier]).Remove(handler);

        public void TriggerEvent<TEvent, TEventArgs>(Event<TEvent, TEventArgs> identifier, object invoker, TEventArgs args) where TEvent : Event<TEvent, TEventArgs>, new() =>
            ((TEvent)_events[identifier]).Invoke(invoker, args);
    }

    public static class EventManagerExtensions
    {//TODO Cache
        public static void RegisterEvent<TEvent>(this EventManager manager) =>
            manager.CallMethod<TEvent>(nameof(RegisterEvent));
        /*
        public static void RegisterEventHandler<TEvent>(this EventManager manager, Delegate eventHandler) => 
            manager.CallMethod<TEvent>(nameof(RegisterEventHandler), eventHandler);

        public static void RemoveEventHandler<TEvent>(this EventManager manager, object eventHandler) =>
            manager.CallMethod<TEvent>(nameof(RemoveEventHandler), eventHandler);
        */
        public static void TriggerEvent<TEvent>(this EventManager manager, object invoker, object args) =>
            manager.CallMethod<TEvent>(nameof(TriggerEvent), invoker, args);

        private static void CallMethod<TEvent>(this EventManager manager, string methodName, params object[] args)
        {
            var eventType = typeof(TEvent);
            manager.CallMethod(eventType, methodName, args);
        }

        private static void CallMethod(this EventManager manager, Type eventType, string methodName, object[] args)
        {
            var e = eventType.GetField("Identifier", BindingFlags.Public | BindingFlags.Static).GetValue(null);
            var managerType = typeof(EventManager);
            managerType
                .GetMethod(methodName)
                .MakeGenericMethod(ExportTypes(e))
                .Invoke(manager, e.AsArray().Concat(args).ToArray());
        }

        private static Type[] ExportTypes(object eventInfo)
        {
            return eventInfo.GetType().GenericTypeArguments;
        }

        public static void RegisterAllInnerEvents<T>(this EventManager manager)
        {
            foreach (var type in typeof(T).GetNestedTypes())
            {
                manager.CallMethod(type, nameof(RegisterEvent), new object[] { type });
            }
        }
    }

}
