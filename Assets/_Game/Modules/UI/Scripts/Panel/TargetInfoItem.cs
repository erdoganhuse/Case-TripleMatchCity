using DG.Tweening;
using Modules.Gameplay.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.UI
{
    public class TargetInfoItem : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _countText;
        
        private int _itemId;
        
        public void Setup(TargetData targetData)
        {
            _itemId = targetData.ItemId;
            _icon.sprite = targetData.Icon;
            SetAmount(targetData.ItemAmount);
        }

        public int GetItemId()
        {
            return _itemId;
        }
        
        public void SetAmount(int count)
        {
            _countText.text = count.ToString();
        }

        public void PlayScaleAnimation()
        {
            transform.DOScale(1.1f, 0.1f).OnComplete(() =>
            {
                transform.DOScale(1f, 0.1f);
            });
        }

        public void PlayCompleteAnimation()
        {
            
        }
    }
}