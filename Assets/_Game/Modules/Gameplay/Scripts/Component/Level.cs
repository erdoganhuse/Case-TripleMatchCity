using System.Collections.Generic;
using System.Linq;
using Modules.Gameplay.Data;
using ThirdParty.Other.EditorButton;
using UnityEngine;

namespace Modules.Gameplay
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private int _duration;
        [SerializeField] private TargetData[] _targets;
        [SerializeField] private MapItem[] _mapItems;

        [EditorButton]
        public void Setup()
        {
            _mapItems = GetComponentsInChildren<MapItem>();
            for (int i = 0; i < _targets.Length; i++)
            {
                int itemId = _targets[i].ItemId;
                _targets[i].Icon = GetItemIcon(itemId);
                _targets[i].CollectScale = GetItemCollectScale(itemId);
            }
        }
        
        public int GetDuration()
        {
            return _duration;
        }
        
        public List<TargetData> GetTargetDatas()
        {
            return _targets.ToList();
        }

        private Sprite GetItemIcon(int itemId)
        {
            for (int i = 0; i < _mapItems.Length; i++)
            {
                if (_mapItems[i].GetId() == itemId)
                {
                    return _mapItems[i].GetIcon();
                }
            }
            
            return null;
        }

        private float GetItemCollectScale(int itemId)
        {
            for (int i = 0; i < _mapItems.Length; i++)
            {
                if (_mapItems[i].GetId() == itemId)
                {
                    return _mapItems[i].GetCollectScale();
                }
            }
            
            return 0f;
        }
    }
}