using System;

namespace Library.ScreenManagement
{
    public abstract class BaseScreenArgs
    {
        public Action OnOpened;
        public Action OnClosed;
    }
    
    public class DefaultScreenArgs : BaseScreenArgs
    {
        public DefaultScreenArgs() { }
        
        public DefaultScreenArgs(Action onOpened, Action onClosed)
        {
            OnOpened = onOpened;
            OnClosed = onClosed;
        }
    }
}