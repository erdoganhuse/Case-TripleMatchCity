using System;
using Library.ScreenManagement;
using TMPro;
using UnityEngine;

namespace Modules.UI
{
    public class PreGameScreenArgs : BaseScreenArgs
    {
        public int LevelNo;
        public Action<int> OnBoosterSelected;
        public Action OnPlay;
    }
    
    public class PreGameScreen : BaseScreen<PreGameScreenArgs>
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        
        public override void OnSetup()
        {
            _titleText.text = $"LEVEL {GetArgs().LevelNo}";
        }

        public void OnCloseButtonClicked()
        {
            Close();
        }
        
        public void OnPlayButtonClicked()
        {
            GetArgs().OnPlay?.Invoke();
        }

        public void OnBoosterButtonClicked(int boosterId)
        {
            GetArgs().OnBoosterSelected?.Invoke(boosterId);
        }
    }
}