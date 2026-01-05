using System;
using System.Collections;
using UnityEngine;

namespace Library.ScreenManagement
{
    public class ScreenOperationOption
    {
        public bool ShouldHideUnderneath { get; private set; } = false;
        public bool ShouldAnimateBg { get; private set; } = true;

        private bool _isCompleted;
        private Action _onComplete;
        
        public ScreenOperationOption()
        {
            _isCompleted = false;
        }

        public void PerformCompleteOperations()
        {
            _isCompleted = true;
            _onComplete?.Invoke();
            _onComplete = null;
        }

        public ScreenOperationOption SetAnimateBg(bool shouldAnimateBg)
        {
            ShouldAnimateBg = shouldAnimateBg;
            return this;
        }
        
        public ScreenOperationOption SetHideUnderneath(bool shouldHideUnderneath)
        {
            ShouldHideUnderneath = shouldHideUnderneath;
            return this;
        }

        public ScreenOperationOption OnComplete(Action callback = null)
        {
            _onComplete = callback;
            return this;
        }
        
        public IEnumerator WaitForCompletion()
        {
            yield return new WaitUntil(() => _isCompleted);
        }

        public static ScreenOperationOption Empty()
        {
            return new ();
        }
    }
}