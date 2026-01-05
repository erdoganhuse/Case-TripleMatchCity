using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Library.SignalBusSystem
{
    public static class SignalBus
    {
        private static readonly Dictionary<Type, List<ListenerData>> SignalTypeByListenersMap = new ();
        private static readonly Dictionary<Type, List<SignalYieldInstruction>> SignalTypeByWaitersMap = new ();

        public static void Subscribe<T>(Action listener, PriorityType priority = PriorityType.Normal) 
            where T : ISignal
        {
            if (!SignalTypeByListenersMap.ContainsKey(typeof(T)))
            {
                SignalTypeByListenersMap.Add(typeof(T), new List<ListenerData>());
            }
                
            SignalTypeByListenersMap[typeof(T)].Add(new ListenerData(priority, listener, listener.Target));
        }        
        
        public static void Subscribe<T>(Action<T> listener, PriorityType priority = PriorityType.Normal) 
            where T : ISignal
        {
            if (!SignalTypeByListenersMap.ContainsKey(typeof(T)))
            {
                SignalTypeByListenersMap.Add(typeof(T), new List<ListenerData>());
            }
            
            Action<ISignal> convertedListener = o => listener((T) o);
            SignalTypeByListenersMap[typeof(T)].Add(new ListenerData(priority, convertedListener, listener.Target));
        }

        public static void Unsubscribe<T>(Action listener) where T : ISignal
        {
            if (!SignalTypeByListenersMap.ContainsKey(typeof(T))) return;

            ListenerData listenerData = SignalTypeByListenersMap[typeof(T)]
                .FirstOrDefault(item => item.Target == listener.Target);

            if (listenerData == null) return;
            
            SignalTypeByListenersMap[typeof(T)].Remove(listenerData);
        }

        public static void Unsubscribe<T>(Action<T> listener) where T : ISignal
        {
            if (!SignalTypeByListenersMap.ContainsKey(typeof(T))) return;

            ListenerData listenerData = SignalTypeByListenersMap[typeof(T)]
                .FirstOrDefault(item => item.Target == listener.Target);

            if (listenerData == null) return;
            
            SignalTypeByListenersMap[typeof(T)].Remove(listenerData);
        }

        public static void Fire<T>(T signal) where T : ISignal
        {
            if (SignalTypeByListenersMap.ContainsKey(signal.GetType()))
            {
                IEnumerable<ListenerData> sortedListeners = SignalTypeByListenersMap[signal.GetType()]
                    .OrderBy(item => item.Priority);

                foreach (var listenerData in sortedListeners)
                {
                    listenerData.Listener?.Invoke();
                    listenerData.ListenerWithParam?.Invoke(signal);
                }
            }

            if (SignalTypeByWaitersMap.ContainsKey(signal.GetType()))
            {
                foreach (var yieldInstruction in SignalTypeByWaitersMap[signal.GetType()])
                {
                    yieldInstruction.Continue();
                }
                
                SignalTypeByWaitersMap[signal.GetType()].Clear();
            }
        }

        public static SignalYieldInstruction WaitForFire<T>()
        {
            if (!SignalTypeByWaitersMap.ContainsKey(typeof(T)))
            {
                SignalTypeByWaitersMap.Add(typeof(T), new List<SignalYieldInstruction>());
            }
         
            SignalYieldInstruction waiter = new SignalYieldInstruction();
            
            SignalTypeByWaitersMap[typeof(T)].Add(waiter);

            return waiter;
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Clear()
        {
            SignalTypeByListenersMap.Clear();   
        }
    }
}