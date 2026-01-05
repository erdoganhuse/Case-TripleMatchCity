using System;

namespace Library.SignalBusSystem
{
    public class ListenerData
    {
        public readonly PriorityType Priority;
        public readonly Action Listener;
        public readonly Action<ISignal> ListenerWithParam;
        public readonly object Target;

        private ListenerData(
            PriorityType priority,
            Action listener,
            Action<ISignal> listenerWithParam,
            object target)
        {
            Priority = priority;
            Listener = listener;
            ListenerWithParam = listenerWithParam;
            Target = target;
        }

        public ListenerData(
            PriorityType priority,
            Action listener,
            object target) : this(priority, listener, null, target) { }
        
        public ListenerData(
            PriorityType priority, 
            Action<ISignal> listenerWithParam,
            object target) : this(priority, null, listenerWithParam, target) { }
    }
}