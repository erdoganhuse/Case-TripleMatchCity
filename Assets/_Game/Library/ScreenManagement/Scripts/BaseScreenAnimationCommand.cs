using System;

namespace Library.ScreenManagement
{
    public abstract class BaseScreenAnimationCommand
    {
        public abstract void PlayOpenAnimation(BaseScreen screen, bool animateBg = true, Action onComplete = null);
        public abstract void PlayCloseAnimation(BaseScreen screen, bool animateBg = true, Action onComplete = null);
    }
}