using System.Collections.Generic;

namespace Library.PersistentData
{
    public class PersistentList<T> : PersistentVariable<List<T>>
    {
        public PersistentList(string saveKey, List<T> defaultValue) : base(saveKey, defaultValue) { }
        
        public void Add(T item, bool shouldSave = true)
        {
            Value.Add(item);
            if (shouldSave)
            {
                Save();
            }
        }

        public void Remove(T item, bool shouldSave = true)
        {
            Value.Remove(item);
            if (shouldSave)
            {
                Save();
            }
        }
    }
}