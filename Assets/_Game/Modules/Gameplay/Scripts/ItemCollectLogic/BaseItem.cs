using UnityEngine;

namespace Modules.Gameplay
{
    public abstract class BaseItem : MonoBehaviour
    {
        public virtual int GetId()
        {
            return -1;
        }
        
        public virtual bool IsInteractable()
        {
            return true;
        }

        public virtual bool IsCollectable()
        {
            return true;
        }

        public virtual float GetCollectLocalScale()
        {
            return 1f;
        }

        public virtual void OnBeforeCollect()
        {
            
        }

        public virtual void OnAfterCollect()
        {
            
        }

        public virtual void OnReturnToMap()
        {
            
        }
    }
}