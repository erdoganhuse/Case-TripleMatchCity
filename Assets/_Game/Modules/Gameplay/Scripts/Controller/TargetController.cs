using System;
using System.Collections.Generic;
using Library.ServiceLocatorSystem;
using Modules.Gameplay.Data;
using UnityEngine;

namespace Modules.Gameplay
{
    public class TargetController : MonoBehaviour
    {
        public event Action OnAllTargetsCompleted;
        public event Action<TargetData> OnTargetInfoChanged;
        public event Action<TargetData> OnTargetCompleted;
        
        private ItemCollectController ItemCollectController => ServiceLocator.Get<ItemCollectController>();

        private List<TargetData> _targets;
        
        public void Setup(List<TargetData> targets)
        {
            _targets = targets;
            RegisterEvents();
        }

        public void Clear()
        {
            UnregisterEvents();
        }

        private bool IsAllTargetsCompleted()
        {
            for (int i = 0; i < _targets.Count; i++)
            {
                if (_targets[i].ItemAmount > 0)
                {
                    return false;
                }
            }

            return true;
        }
        
        public bool IsTargetItem(int itemId)
        {
            return GetTargetDataWithId(itemId) != null;
        }

        private bool IsTargetCompleted(int itemId)
        {
            return IsTargetItem(itemId) && GetTargetDataWithId(itemId).ItemAmount <= 0;
        }
        
        public int GetTargetCount()
        {
            return _targets.Count;
        }
        
        public TargetData GetTargetDataWithIndex(int index)
        {
            return _targets[index];
        }
        
        public TargetData GetTargetDataWithId(int itemId)
        {
            for (int i = 0; i < _targets.Count; i++)
            {
                if (_targets[i].ItemId == itemId)
                {
                    return _targets[i];
                }
            }

            return null;
        }
        
        private void ChangeTargetAmount(int itemId, int changeAmount)
        {
            for (int i = 0; i < _targets.Count; i++)
            {
                if (_targets[i].ItemId == itemId)
                {
                    _targets[i].ItemAmount += changeAmount;
                }
            }
        }
        
        #region Event Methods

        private void RegisterEvents()
        {
            ItemCollectController.OnItemCollected += OnItemCollected;
            ItemCollectController.OnItemReturned += OnItemReturned;
        }

        private void UnregisterEvents()
        {
            ItemCollectController.OnItemCollected -= OnItemCollected;
            ItemCollectController.OnItemReturned -= OnItemReturned;
        }

        private void OnItemCollected(int itemId)
        {
            if (!IsTargetItem(itemId) || IsTargetCompleted(itemId)) return;
            
            ChangeTargetAmount(itemId, -1);
            TargetData targetData = GetTargetDataWithId(itemId);
            
            OnTargetInfoChanged?.Invoke(targetData);

            if (IsTargetCompleted(itemId))
            {
                OnTargetCompleted?.Invoke(targetData);

                if (IsAllTargetsCompleted())
                {
                    OnAllTargetsCompleted?.Invoke();
                }
            }
        }

        private void OnItemReturned(int itemId)
        {
            if (!IsTargetItem(itemId)) return;

            ChangeTargetAmount(itemId, 1);

            TargetData targetData = GetTargetDataWithId(itemId);
            
            OnTargetInfoChanged?.Invoke(targetData);
        }
        
        #endregion        
    }
}