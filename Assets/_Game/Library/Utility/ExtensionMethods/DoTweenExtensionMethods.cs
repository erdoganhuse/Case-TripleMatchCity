using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Library.Utility
{
    public static class DoTweenExtensionMethods
    {
        private static List<Tween> _attentionJumpTweens = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Clear()
        {
            _attentionJumpTweens.Clear();
        }
        
        public static Tween DOScaleJump(this Transform transform, float scaleChange = 1.1f, float duration = 0.3f)
        {
            transform.DOKill(true);
            Vector3 startScale = transform.localScale;
            return DOTween.Sequence()
                .Append(transform.DOScale(startScale * scaleChange, duration * (1f / 3f)))
                .Append(transform.DOScale(startScale, duration * (2f / 3f)));
        }

        public static Tween DOCubicBezierPath(this Transform transform, Vector3 fromPos, Vector3 toPos, float duration, 
            float multiplier = 1f,
            float normalizedMiddlePoint = 0.325f)
        {
            Vector3 p1 = fromPos;
            Vector3 p3 = toPos;
            Vector3 p2 = Vector3.Lerp(p1, p3, normalizedMiddlePoint) + Vector3.up * (2f * multiplier);
            
            Vector3 a = p1 + Vector3.up * (1f * multiplier);
            Vector3 b = (p1 - p3).normalized + p2;
            Vector3 c = (p3 - p1).normalized + p2;
            Vector3 d = p3 + Vector3.up * (2f * multiplier);
            
            Vector3[] pathPoints = { p2, a, b, p3, c, d };

            return transform.DOPath(pathPoints, duration, PathType.CubicBezier);
        }        
        
        public static Tween DOLocalCubicBezierPath(this Transform transform, Vector3 fromPos, Vector3 toPos, float duration, 
            float multiplier = 1f,
            float normalizedMiddlePoint = 0.325f)
        {
            Vector3 p1 = fromPos;
            Vector3 p3 = toPos;
            Vector3 p2 = Vector3.Lerp(p1, p3, normalizedMiddlePoint) + Vector3.up * (2f * multiplier);
            
            Vector3 a = p1 + Vector3.up * (1f * multiplier);
            Vector3 b = (p1 - p3).normalized + p2;
            Vector3 c = (p3 - p1).normalized + p2;
            Vector3 d = p3 + Vector3.up * (2f * multiplier);
            
            Vector3[] pathPoints = { p2, a, b, p3, c, d };

            return transform.DOLocalPath(pathPoints, duration, PathType.CubicBezier);
        }

        public static Tween DOAttentionRotate(this RectTransform rectTransform, float angleZ, float duration, int stepCount = 4)
        {
            Sequence sequence = DOTween.Sequence();
            float stepDuration = duration / stepCount;
            sequence.Append(rectTransform.DOLocalRotateQuaternion(Quaternion.Euler(0f, 0f, angleZ), stepDuration/2f));
            for (int i = 1; i < stepCount; i++)
            {
                float stepAngleZ = i % 2 == 0 ? angleZ : -angleZ;
                sequence.Append(rectTransform.DOLocalRotateQuaternion(Quaternion.Euler(0f, 0f, stepAngleZ), stepDuration));
            }
            sequence.Append(rectTransform.DOLocalRotateQuaternion(Quaternion.Euler(0f, 0f, 0f), stepDuration/2f));
            sequence.OnKill(() =>
            {
                rectTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            });
            return sequence;
        }
        
        public static Tween DOAttentionJump(this RectTransform rectTransform, float duration = 1.6f)
        {
            Vector2 initialAnchorPos = rectTransform.anchoredPosition;
            float moveDuration = duration * (2f / 7f);
            float rotateDuration = duration * (3f / 7f);
            
            Sequence sequence = DOTween.Sequence();
            sequence.Append(rectTransform.DOAnchorPosY(initialAnchorPos.y + 50f, moveDuration).SetEase(Ease.OutSine));
            sequence.Append(rectTransform.DOAttentionRotate(20f, rotateDuration));
            sequence.Append(rectTransform.DOAnchorPosY(initialAnchorPos.y, moveDuration).SetEase(Ease.InSine));
            sequence.SetLoops(-1, LoopType.Restart);
            sequence.OnKill(() =>
            {
                rectTransform.anchoredPosition = initialAnchorPos;
            });

            for (int i = _attentionJumpTweens.Count - 1; i >= 0; i--)
            {
                if (_attentionJumpTweens[i] != null)
                {
                    _attentionJumpTweens[i].Restart();
                }
                else
                {
                    _attentionJumpTweens.RemoveAt(i);   
                }
            }
            _attentionJumpTweens.Add(sequence);
            
            return sequence;
        }
    }
}