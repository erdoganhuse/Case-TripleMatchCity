using System.Collections.Generic;
using Library.ServiceLocatorSystem;
using Library.Utility;
using Modules.Gameplay;
using Modules.Gameplay.Data;
using UnityEngine;

namespace Modules.UI
{
    public class TargetInfoPanel : MonoBehaviour
    {
        private TargetController TargetController => ServiceLocator.Get<TargetController>();

        [SerializeField] private TargetInfoItem _targetInfoItemPrefab;
        [SerializeField] private Transform _container;

        private List<TargetInfoItem> _activeInfoItems = new();
        private UnityObjectPool<TargetInfoItem> _targetInfoItemPool;
        
        private void Start()
        {
            TargetController.OnTargetInfoChanged += OnTargetInfoChanged;
            TargetController.OnTargetCompleted += OnTargetCompleted;
        }

        private void OnDestroy()
        {
            TargetController.OnTargetInfoChanged -= OnTargetInfoChanged;
            TargetController.OnTargetCompleted -= OnTargetCompleted;
        }
        
        public void Setup()
        {
            if (_targetInfoItemPool == null)
            {
                _targetInfoItemPool = new UnityObjectPool<TargetInfoItem>(_targetInfoItemPrefab, 5, _container);
            }

            int targetAmount = TargetController.GetTargetCount();
            for (int i = 0; i < targetAmount; i++)
            {
                TargetData targetData = TargetController.GetTargetDataWithIndex(i);
                TargetInfoItem infoItem = _targetInfoItemPool.Spawn();
                infoItem.Setup(targetData);
                infoItem.gameObject.SetActive(true);
                _activeInfoItems.Add(infoItem);
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _activeInfoItems.Count; i++)
            {
                _targetInfoItemPool.Despawn(_activeInfoItems[i]);
            }
            _activeInfoItems.Clear();
        }
        
        private void OnTargetInfoChanged(TargetData targetData)
        {
            for (int i = 0; i < _activeInfoItems.Count; i++)
            {
                if (_activeInfoItems[i].GetItemId() == targetData.ItemId)
                {
                    _activeInfoItems[i].SetAmount(targetData.ItemAmount);
                    _activeInfoItems[i].PlayScaleAnimation();
                }
            }
        }

        private void OnTargetCompleted(TargetData targetData)
        {
            for (int i = 0; i < _activeInfoItems.Count; i++)
            {
                if (_activeInfoItems[i].GetItemId() == targetData.ItemId)
                {
                    _activeInfoItems[i].PlayCompleteAnimation();
                }
            }
        }
    }
}