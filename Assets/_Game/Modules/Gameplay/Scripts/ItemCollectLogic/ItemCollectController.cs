using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Library.Utility;
using UnityEngine;

namespace Modules.Gameplay
{
    public class ItemCollectController : MonoBehaviour
    {
        private const int InitialSlotCount = 7;
        private const int RequiredItemCountForMerge = 3;

        public event Action<int> OnItemCollected;
        public event Action<int> OnItemReturned;
        public event Action OnAllSlotsFilled;
        
        [SerializeField] private Slot _slotPrefab;
        [SerializeField] private Transform _slotContainer;

        private List<Slot> _currentSlots;
        private UnityObjectPool<Slot> _slotPool;

        private bool _isDuringCollect;
        private bool _isDuringMerge;
        private bool _isDuringShift;
        
        public void Setup()
        {
            if (_slotPool == null)
            {
                _slotPool = new UnityObjectPool<Slot>(_slotPrefab, InitialSlotCount, _slotContainer);
            }
            
            _currentSlots = new List<Slot>(InitialSlotCount);
            for (int i = 0; i < InitialSlotCount; i++)
            {
                Slot slot = _slotPool.Spawn();
                slot.gameObject.SetActive(true);
                _currentSlots.Add(slot);
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _currentSlots.Count; i++)
            {
                if (!_currentSlots[i].IsEmpty())
                {
                    DestroyImmediate(_currentSlots[i].CurrentItem.gameObject);
                }
                _currentSlots[i].Clear();
                _slotPool.Despawn(_currentSlots[i]);
            }
            _currentSlots.Clear();
        }
        
        public bool CanCollect(BaseItem item)
        {
            return IsEmptySlotExist();
        }
        
        public async UniTask CollectAsync(BaseItem item)
        {
            item.OnBeforeCollect();
         
            OnItemCollected?.Invoke(item.GetId());
            
            ArrangeSlotsForItem(item);
            
            Slot slot = GetSuitablePlacementSlot();
            slot.Place(item);

            List<BaseItem> itemsWithSameId = GetPlacedItemsWithId(item.GetId());
            bool isMergeExist = itemsWithSameId.Count >= RequiredItemCountForMerge;
            
            _isDuringCollect = true;
            await PlayItemCollectAnimationAsync(item, slot);
            _isDuringCollect = false;
            
            if (isMergeExist)
            {
                await UniTask.WaitUntil(() => !_isDuringCollect);

                _isDuringMerge = true;
                MergeItems(itemsWithSameId);
                await PlayItemsMergeAnimationAsync(itemsWithSameId);
                _isDuringMerge = false;

                await UniTask.WaitUntil(() => !_isDuringCollect && !_isDuringMerge && !_isDuringShift);

                _isDuringShift = true;
                ShiftSlotItems();
                await PlaySlotItemsShiftAnimationAsync();
                _isDuringShift = false;
            }
            
            item.OnAfterCollect();

            if (IsAllSlotsFilled())
            {
                OnAllSlotsFilled?.Invoke();
            }
        }

        public void ClearAllSlots()
        {
            for (int i = 0; i < _currentSlots.Count; i++)
            {
                Slot slot = _currentSlots[i];
                if (!slot.IsEmpty())
                {
                    BaseItem item = slot.CurrentItem;
                    item.OnReturnToMap();
                    slot.Clear();
                    
                    OnItemReturned?.Invoke(item.GetId());
                }
            }
        }
        
        #region Merge Methods

        private void MergeItems(List<BaseItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Slot placedSlot = GetPlacedSlot(items[i]);
                placedSlot.Clear();
            }
        }
        
        #endregion
        
        #region Slot Methods
        
        private bool IsEmptySlotExist()
        {
            for (int i = 0; i < _currentSlots.Count; i++)
            {
                if (_currentSlots[i].IsEmpty())
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsAllSlotsFilled()
        {
            for (int i = 0; i < _currentSlots.Count; i++)
            {
                if (_currentSlots[i].IsEmpty()) return false;
            }

            return true;
        }
        
        private void ShiftSlotItems()
        {
            List<BaseItem> items = new();
            for (int i = 0; i < _currentSlots.Count; i++)
            {
                if (!_currentSlots[i].IsEmpty())
                {
                    items.Add(_currentSlots[i].CurrentItem);
                }
            }

            for (int i = 0; i < _currentSlots.Count; i++)
            {
                if (i < items.Count)
                {
                    _currentSlots[i].Place(items[i]);
                }
                else
                {
                    _currentSlots[i].Clear();
                }
            }
        }
        
        private void ArrangeSlotsForItem(BaseItem item)
        {
            int placementSlotIndex = -1;
            for (int i = _currentSlots.Count - 1; i >= 0; i--)
            {
                Slot slot = _currentSlots[i];
                if (!slot.IsEmpty() && slot.CurrentItem.GetId() == item.GetId())
                {
                    placementSlotIndex = i + 1;
                    break;
                }
            }

            if (placementSlotIndex != -1)
            {
                for (int i = _currentSlots.Count - 2; i >= placementSlotIndex; i--)
                {
                    Slot slot = _currentSlots[i];
                    Slot nextSlot = _currentSlots[i + 1];
                    
                    if (!slot.IsEmpty())
                    {
                        nextSlot.Place(slot.CurrentItem);
                        nextSlot.CurrentItem.transform.DOLocalMove(Vector3.zero, 0.2f);
                        slot.Clear();
                    }
                }
                
                _currentSlots[placementSlotIndex].Clear();
            }
        }
        
        private Slot GetSuitablePlacementSlot()
        {
            for (int i = 0; i < _currentSlots.Count; i++)
            {
                if (_currentSlots[i].IsEmpty())
                {
                    return _currentSlots[i];
                }
            }

            return null;
        }

        private Slot GetPlacedSlot(BaseItem item)
        {
            for (int i = 0; i < _currentSlots.Count; i++)
            {
                if (_currentSlots[i].CurrentItem == item)
                {
                    return _currentSlots[i];
                }
            }

            return null;
        }
        
        private List<BaseItem> GetPlacedItemsWithId(int itemId)
        {
            List<BaseItem> placedItems = new ();
            for (int i = 0; i < _currentSlots.Count; i++)
            {
                if (!_currentSlots[i].IsEmpty() &&
                    _currentSlots[i].CurrentItem.GetId() == itemId)
                {
                    placedItems.Add(_currentSlots[i].CurrentItem);
                }
            }

            return placedItems;
        }
        
        #endregion

        #region Animation Methods

        private async UniTask PlayItemCollectAnimationAsync(BaseItem item, Slot slot)
        {
            Vector3 fromLocalPos = item.transform.localPosition;
            Vector3 toLocalPos = new Vector3(0f, 0f, 0f);
            float toLocalScale = item.GetCollectLocalScale();
            
            Sequence sequence = DOTween.Sequence();
            sequence.Append(item.transform.DOScale(toLocalScale, 0.6f));
            sequence.Join(item.transform.DOLocalMove(toLocalPos, 0.6f).SetEase(Ease.InBack, 2f));
            
            await sequence.AsyncWaitForCompletion();
        }

        private async UniTask PlayItemsMergeAnimationAsync(List<BaseItem> items)
        {
            Vector3 center = Vector3.zero;
            for (int i = 0; i < items.Count; i++)
            {
                center += items[i].transform.position;
            }
            
            center /= items.Count;
            
            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < items.Count; i++)
            {
                sequence.Insert(0f, items[i].transform.DOMove(center, 0.2f));
            }
            
            await sequence.AsyncWaitForCompletion();

            for (int i = 0; i < items.Count; i++)
            {
                Destroy(items[i].gameObject);
            }
        }
                
        private async UniTask PlaySlotItemsShiftAnimationAsync()
        {
            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < _currentSlots.Count; i++)
            {
                if (!_currentSlots[i].IsEmpty())
                {
                    BaseItem item = _currentSlots[i].CurrentItem;
                    sequence.Insert(0f, item.transform.DOLocalMove(Vector3.zero, 0.2f));
                }
            }
            
            await sequence.AsyncWaitForCompletion();
        }
        
        #endregion
    }
}