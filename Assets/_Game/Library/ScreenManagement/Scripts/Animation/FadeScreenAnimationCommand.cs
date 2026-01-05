using System;
using DG.Tweening;
using Library.Utility;

namespace Library.ScreenManagement
{
    public class FadeScreenAnimationCommand : BaseScreenAnimationCommand
    {
        private readonly float _duration;
        private readonly float _bgAlpha;
        
        public FadeScreenAnimationCommand(float duration = 0.3f, float bgAlpha = 0.95f)
        {
            _duration = duration;
            _bgAlpha = bgAlpha;
        }
        
        public override void PlayOpenAnimation(BaseScreen screen, bool animateBg = true, Action onComplete = null)
        {
            screen.Content.alpha = 0f;
            
            if (screen.Background != null)
            {
                screen.Background.SetAlpha(0f);
            }
            
            Sequence sequence = DOTween.Sequence();
            sequence.Append(screen.Content.DOFade(1f, _duration));
            
            if (screen.Background != null)
            {
                if (animateBg)
                {
                    sequence.Join(screen.Background.DOFade(_bgAlpha, _duration));   
                }
                else
                {
                    screen.Background.SetAlpha(_bgAlpha);
                }
            }

            sequence.OnComplete(() => { onComplete?.Invoke(); });
        }

        public override void PlayCloseAnimation(BaseScreen screen, bool animateBg = true, Action onComplete = null)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(screen.Content.DOFade(0f, _duration));
            
            if (screen.Background != null)
            {
                if (animateBg)
                {
                    sequence.Join(screen.Background.DOFade(0f, _duration));   
                }
                else
                {
                    screen.Background.SetAlpha(0f);
                }
            }

            sequence.OnComplete(() => { onComplete?.Invoke(); });
        }
    }
}