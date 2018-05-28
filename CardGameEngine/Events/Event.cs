using System;
using System.Collections.Generic;
using System.Text;

namespace CardGameEngine.Events
{
    public abstract class Event
    {
        private event EventHandler InnerEvent;

        public void Register(EventHandler eventHandler)
        {
            InnerEvent += eventHandler;
        }

        public void Remove(EventHandler eventHandler)
        {
            InnerEvent -= eventHandler;
        }

        public void Invoke(object invoker, EventArgs args)
        {
            InnerEvent?.Invoke(invoker, args);
        }
    }
    
}
