using Newtonsoft.Json;
using UnityEngine;

namespace Library.PersistentData
{
    public class PersistentVariable<T>
    {
        public T Value
        {
            get
            {
                if (!_isLoaded)
                {
                    if (!PlayerPrefs.HasKey(_saveKey))
                    {
                        Save();
                    }
                    
                    Load();
                }

                return _value;
            }
            set
            {
                _value = value;
                Save();
            }
        }

        protected T _value;
        protected readonly string _saveKey;
        
        protected bool _isLoaded;
        
        public PersistentVariable(string saveKey, T defaultValue)
        {
            _value = defaultValue;
            _saveKey = saveKey;
        }

        protected virtual void Save()
        {
            string json = JsonConvert.SerializeObject(_value, Formatting.Indented);
            PlayerPrefs.SetString(_saveKey, json);
            PlayerPrefs.Save();
        }

        protected virtual void Load()
        {
            _isLoaded = true;
            
            if (!PlayerPrefs.HasKey(_saveKey))
            {
                return;
            }
            
            string json = PlayerPrefs.GetString(_saveKey);
            _value = JsonConvert.DeserializeObject<T>(json);
        }

        public virtual void ForceSave()
        {
            Save();   
        }
        
        public static implicit operator T(PersistentVariable<T> persistentVariable)
        {
            return persistentVariable.Value;
        }
    }
}