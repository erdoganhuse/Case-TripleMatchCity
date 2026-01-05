using UnityEngine;

namespace Library.PersistentData
{
    public class PersistentFloat : PersistentVariable<float>
    {
        public PersistentFloat(string saveKey, float defaultValue = default) : base(saveKey, defaultValue) { }

        protected override void Save()
        {
            PlayerPrefs.SetFloat(_saveKey, _value);
        }

        protected override void Load()
        {
            _value = PlayerPrefs.GetFloat(_saveKey);
        }
    }
}