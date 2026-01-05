using System;
using System.Collections;
using Library.CoroutineSystem;

namespace Library.ScreenManagement
{
    public class CoroutineScreenAnimationCommand : BaseScreenAnimationCommand
    {
        private readonly IEnumerator _openContentAnimCoroutine;
        private readonly IEnumerator _openBgAnimCoroutine;
        private readonly IEnumerator _closeContentAnimCoroutine;
        private readonly IEnumerator _closeBgAnimCoroutine;
        
        public CoroutineScreenAnimationCommand(
            IEnumerator openBgAnimCoroutine,
            IEnumerator openContentAnimCoroutine,
            IEnumerator closeBgAnimCoroutine,
            IEnumerator closeContentAnimCoroutine)
        {
            _openBgAnimCoroutine = openBgAnimCoroutine;
            _openContentAnimCoroutine = openContentAnimCoroutine;
            _closeBgAnimCoroutine = closeBgAnimCoroutine;
            _closeContentAnimCoroutine = closeContentAnimCoroutine;
        }
        
        public override void PlayOpenAnimation(BaseScreen screen, bool animateBg = true, Action onComplete = null)
        {
            CoroutineManager.BeginCoroutine(OpenCoroutine(screen, animateBg, onComplete));
        }

        public override void PlayCloseAnimation(BaseScreen screen, bool animateBg = true, Action onComplete = null)
        {
            CoroutineManager.BeginCoroutine(CloseCoroutine(screen, animateBg, onComplete));
        }

        private IEnumerator OpenCoroutine(BaseScreen screen, bool animateBg = true, Action onComplete = null)
        {
            screen.Content.blocksRaycasts = false;
            
            if(animateBg)
            {
                yield return _openBgAnimCoroutine;
            }
            
            yield return _openContentAnimCoroutine;

            screen.Content.blocksRaycasts = true;
            
            onComplete?.Invoke();
        }        
        
        private IEnumerator CloseCoroutine(BaseScreen screen, bool animateBg = true, Action onComplete = null)
        {
            if(animateBg)
            {
                yield return _closeBgAnimCoroutine;
            }
            
            yield return _closeContentAnimCoroutine;
            
            onComplete?.Invoke();
        }
    }
}