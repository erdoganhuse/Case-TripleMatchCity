using UnityEngine;

namespace Library.PersistentData
{
    public class PersistentInt : PersistentVariable<int>
    {
        public PersistentInt(string saveKey, int defaultValue = default) : base(saveKey, defaultValue) { }

        protected override void Save()
        {
            PlayerPrefs.SetInt(_saveKey, _value);
        }

        protected override void Load()
        {
            _value = PlayerPrefs.GetInt(_saveKey);
        }
    }
}