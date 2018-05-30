using System;
using System.Collections.Generic;
using System.Diagnostics;
using CardGameEngine.Utils;
using GammaLibrary;
using Humanizer;

namespace CardGameEngine.Events
{
    public abstract class Event<TEvent, TEventArgs> where TEvent : Event<TEvent, TEventArgs>, new()
    {
        private readonly List<EventInfo> Events = new List<EventInfo>();
        public static readonly Event<TEvent, TEventArgs> Identifier;
        private bool _locked;

        private class EventInfo
        {
            public EventHandler<TEventArgs> Handler;
            public string StackTraceWhenRegistering;
            public DateTime TimeWhenRegistering;

            public EventInfo(EventHandler<TEventArgs> handler)
            {
                Handler = handler;
                StackTraceWhenRegistering = Environment.StackTrace;
                TimeWhenRegistering = DateTime.Now;
            }

            public string HandlerInfo => Handler?.Method.ToString();
        }

        static Event()
        {
            Identifier = new TEvent {_locked = true};
        }

        public void Register(EventHandler<TEventArgs> eventHandler)
        {
            if (_locked) throw new InvalidOperationException();
            Events.Add(new EventInfo(eventHandler));
        }

        public void Remove(EventHandler<TEventArgs> eventHandler)
        {
            if (_locked) throw new InvalidOperationException();
            Events.RemoveAll(e => e.Handler == eventHandler);
        }

        public void Invoke(object invoker, TEventArgs args)
        {
            if (_locked) throw new InvalidOperationException();

            Trace.TraceInformation($"Begin event handle in type {typeof(TEvent)}");
            Trace.Indent();
            foreach (var handler in Events)
            {
                Benchmark.MeasureTime(() =>
                    handler.Handler.InvokeWhenException(invoker, args,
                        e => Trace.TraceError($"\t" +
                                              $"An exception has occurred in the event {typeof(TEvent)}." + Environment.NewLine +
                                              $"Stack trace when registering: {handler.StackTraceWhenRegistering}" + Environment.NewLine +
                                              $"Time when Registering: {handler.TimeWhenRegistering}" + Environment.NewLine +
                                              $"Exception: {e}" + Environment.NewLine)),
                        span => Trace.TraceInformation($"Event handler call finished in {span.Humanize()}"));
            }
            Trace.Unindent();
            Trace.TraceInformation($"{Events.Count} event{(Events.Count > 1 ? "s" :string.Empty)} performed in type {typeof(TEvent)}.");
        }
    }
    
}
