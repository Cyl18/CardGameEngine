using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace CardGameEngine.Events
{
    public class EventManager
    {
        // Object or Dynamic????
        private readonly ConcurrentDictionary<Type, object> _events = new ConcurrentDictionary<Type, object>();

        // 傻逼 C# 泛型
        public void RegisterEvent<TEvent>() where TEvent : Event, new()
        {
            _events.TryAdd(typeof(TEvent), new TEvent());
        }

        public void RegisterEventHandler<TEvent>(EventHandler eventHandler) where TEvent : Event
        {
            var e = (Event) _events[typeof(TEvent)];
            e.Register(eventHandler);
        }

        public void RemoveEventHandler<TEvent>(EventHandler eventHandler) where TEvent : Event
        {
            var e = (Event)_events[typeof(TEvent)];
            e.Remove(eventHandler);
        }

        public void TriggerEvent<TEvent>(object invoker, EventArgs args) where TEvent : Event
        {
            var e = (Event)_events[typeof(TEvent)];
            e.Invoke(invoker, args);
        }
    }
    
}
