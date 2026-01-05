using System;
using Library.ScreenManagement;

namespace Modules.UI
{
    public class OutOfSpaceScreenArgs : BaseScreenArgs
    {
        public Action OnGiveUp;
        public Action OnClearSpace;
    }
    
    public class OutOfSpaceScreen : BaseScreen<OutOfSpaceScreenArgs>
    {
        public void OnCloseButtonClicked()
        {
            GetArgs().OnGiveUp?.Invoke();
        }

        public void OnClearSpaceButtonClicked()
        {
            GetArgs().OnClearSpace?.Invoke();
        }
    }
}