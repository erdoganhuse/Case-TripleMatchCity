using System;
using System.Collections.Generic;

namespace Library.ServiceLocatorSystem
{
    public class BindingData
    {
        private readonly Type _type;
        private readonly string _id;
        
        public BindingData(Type type, string id = "")
        {
            _type = type;
            _id = id;
        }
        
        public class EqualityComparer : IEqualityComparer<BindingData> 
        {
            public bool Equals(BindingData x, BindingData y)
            {
                return x._type == y._type && x._id == y._id;
            }

            public int GetHashCode(BindingData obj)
            {
                return HashCode.Combine(obj._type, obj._id);
            }
        }
    }
}