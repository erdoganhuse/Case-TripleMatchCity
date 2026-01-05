using System;

namespace Library.PersistentData
{
    public class PersistentDate : PersistentVariable<DateTime>
    {
        public PersistentDate(string saveKey, DateTime defaultValue) : base(saveKey, defaultValue)
        {
        }
    }
}