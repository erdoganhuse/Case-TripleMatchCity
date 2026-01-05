using System;
using DG.Tweening;
using Library.Utility;
using UnityEngine;

namespace Library.ScreenManagement
{
    public class DefaultScreenAnimationCommand : BaseScreenAnimationCommand
    {
        private readonly float _bgFadeAlpha;
        private readonly float _durationMultiplier;
        
        public DefaultScreenAnimationCommand(float bgFadeAlpha = 0.95f, float durationMultiplier = 1f)
        {
            _bgFadeAlpha = bgFadeAlpha;
            _durationMultiplier = durationMultiplier;
        }
        
        public override void PlayOpenAnimation(BaseScreen screen, bool animateBg = true, Action onComplete = null)
        {
            screen.Background.SetAlpha(0.4f);
            screen.Content.transform.localScale = 0.8f * Vector3.one;
            screen.Content.alpha = 0.5f;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(screen.Content.transform.DOScale(1f, 0.4f * _durationMultiplier)
                .SetEase(Ease.OutBack, 2.5f));
            sequence.Join(screen.Content.DOFade(1f, 0.15f * _durationMultiplier).SetEase(Ease.Linear));

            if (animateBg)
            {
                sequence.Join(screen.Background.DOFade(_bgFadeAlpha, 0.1f * _durationMultiplier)
                    .SetEase(Ease.Linear));
            }
            else
            {
                screen.Background.SetAlpha(_bgFadeAlpha);
            }
            sequence.OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }

        public override void PlayCloseAnimation(BaseScreen screen, bool animateBg = true, Action onComplete = null)
        {
            screen.Content.transform.localScale = 0 * Vector3.one;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(screen.Background.DOFade(0f, 0.2f));
            sequence.OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }
    }
}