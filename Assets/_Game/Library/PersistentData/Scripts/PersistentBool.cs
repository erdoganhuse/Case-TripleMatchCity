using UnityEngine;

namespace Library.PersistentData
{
    public class PersistentBool : PersistentVariable<bool>
    {
        public PersistentBool(string saveKey, bool defaultValue = default) : base(saveKey, defaultValue) { }

        protected override void Save()
        {
            PlayerPrefs.SetInt(_saveKey, _value ? 1 : 0);
        }

        protected override void Load()
        {
            _value = PlayerPrefs.GetInt(_saveKey) == 1;
        }
    }
}