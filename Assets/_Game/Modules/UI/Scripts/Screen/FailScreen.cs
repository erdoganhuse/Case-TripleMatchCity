using System;
using Library.ScreenManagement;

namespace Modules.UI
{
    public class FailScreenArgs : BaseScreenArgs
    {
        public Action<int> OnBoosterSelected;
        public Action OnTryAgain;
    }
    
    public class FailScreen : BaseScreen<FailScreenArgs>
    {
        public void OnTryAgainButtonClicked()
        {
            GetArgs().OnTryAgain?.Invoke();
        }

        public void OnBoosterButtonClicked(int boosterId)
        {
            GetArgs().OnBoosterSelected?.Invoke(boosterId);
        }   
    }
}