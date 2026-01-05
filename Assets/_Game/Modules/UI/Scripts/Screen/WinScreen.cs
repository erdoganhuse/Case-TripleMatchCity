using System;
using Library.ScreenManagement;
using TMPro;
using UnityEngine;

namespace Modules.UI
{
    public class WinScreenArgs : BaseScreenArgs
    {
        public int GainedStarCount;
        public string RemainingDurationText;
        public Action OnContinue;
    }
    
    public class WinScreen : BaseScreen<WinScreenArgs>
    {
        [SerializeField] private Transform[] _stars;
        [SerializeField] private TextMeshProUGUI _remainingDurationText;

        public override void OnSetup()
        {
            _remainingDurationText.text = GetArgs().RemainingDurationText;
            for (int i = 0; i < GetArgs().GainedStarCount; i++)
            {
                bool isStarVisual = GetArgs().GainedStarCount > i;
                _stars[i].gameObject.SetActive(isStarVisual);
            }
        }

        public void OnContinueButtonClicked()
        {
            GetArgs().OnContinue?.Invoke();
        }
    }
}