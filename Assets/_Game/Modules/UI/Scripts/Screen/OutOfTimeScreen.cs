using System;
using Library.ScreenManagement;
using TMPro;
using UnityEngine;

namespace Modules.UI
{
    public class OutOfTimeScreenArgs : BaseScreenArgs
    {
        public string AdditionalTimeText;
        public Action OnGiveUp;
        public Action OnAddTime;
    }
    
    public class OutOfTimeScreen : BaseScreen<OutOfTimeScreenArgs>
    {
        [SerializeField] private TextMeshProUGUI _additionalTimeText;

        public override void OnSetup()
        {
            _additionalTimeText.text = GetArgs().AdditionalTimeText;
        }

        public void OnCloseButtonClicked()
        {
            GetArgs().OnGiveUp?.Invoke();
        }

        public void OnAddTimeButtonClicked()
        {
            GetArgs().OnAddTime?.Invoke();
        }
    }
}