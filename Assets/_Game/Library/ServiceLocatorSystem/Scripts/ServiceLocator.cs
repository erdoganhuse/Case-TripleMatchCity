using System.Collections.Generic;
using UnityEngine;

namespace Library.ServiceLocatorSystem
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<BindingData, System.Object> BindingByInstanceMap = new(new BindingData.EqualityComparer());

        public static T Get<T>(string id = "")
        {
            return (T)BindingByInstanceMap[new BindingData(typeof(T), id)];
        }
        
        public static void Bind<TInterface, TImplementation>(TImplementation instance, string id = "")
            where TImplementation : TInterface
            where TInterface : class 
        {
            BindingByInstanceMap[new BindingData(typeof(TInterface), id)] = instance;
        }
        
        public static void Bind<TInterface>(TInterface instance, string id = "") where TInterface : class 
        {
            BindingByInstanceMap[new BindingData(typeof(TInterface), id)] = instance;
        }
        
        public static void Bind<TInterface, TImplementation>(string id = "")
            where TImplementation : TInterface, new()
            where TInterface : class
        {
            Bind<TInterface, TImplementation>(new TImplementation(), id);
        }
        
        public static void BindInstance<TImplementation>(TImplementation instance, string id = "")
            where TImplementation : class
        {
            Bind<TImplementation, TImplementation>(instance, id);
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Clear()
        {
            BindingByInstanceMap.Clear();
        }
    }
}