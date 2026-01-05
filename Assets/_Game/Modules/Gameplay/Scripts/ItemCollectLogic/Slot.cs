using UnityEngine;

namespace Modules.Gameplay
{
    public class Slot : MonoBehaviour
    {
        public BaseItem CurrentItem { get; private set; }
        
        [SerializeField] private Transform _container;

        public bool IsEmpty()
        {
            return CurrentItem == null;
        }
        
        public void Place(BaseItem item)
        {
            CurrentItem = item;
            CurrentItem.transform.SetParent(_container, true);
        }

        public void Clear()
        {
            CurrentItem = null;
        }
    }
}