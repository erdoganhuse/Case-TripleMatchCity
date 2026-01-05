using System;
using System.Collections;
using Library.CoroutineSystem;
using UnityEngine;

namespace Modules.Gameplay
{
    public class CountdownController : MonoBehaviour
    {
        public event Action OnRemainingTimeChanged;
        public event Action OnCountdownEnded;
        
        public bool IsActive { get; private set; }
        public float RemainingTimeInSec { get; private set; }
        
        private bool _isPaused;
        private float _initialLevelDuration;
        private float _totalLevelDuration;
        private Coroutine _countdownCoroutine;
        
        public void Setup(float duration)
        {
            IsActive = true;
            
            _initialLevelDuration = duration;
            _totalLevelDuration = _initialLevelDuration;
            
            RemainingTimeInSec = _totalLevelDuration;
        }

        public void Clear()
        {
            if (!IsActive) { return; }
            
            IsActive = false;
            
            CoroutineManager.EndCoroutine(_countdownCoroutine);
        }

        #region Main Methods
        
        public void StartCountdown()
        {
            _isPaused = false;
            _countdownCoroutine = CoroutineManager.BeginCoroutine(CountdownCoroutine());
        }
        
        public void PauseCountdown()
        {
            _isPaused = true;
        }

        public void ResumeCountdown()
        {
            _isPaused = false;
        }

        public void AddDurationToCountdown(float amountInSec)
        {
            _totalLevelDuration += amountInSec;
            RemainingTimeInSec += amountInSec;
            RemainingTimeInSec = Mathf.Max(0f, RemainingTimeInSec);
            
            OnRemainingTimeChanged?.Invoke();
        }

        public void RemoveDurationFromCountdown(float amountInSec)
        {
            RemainingTimeInSec -= amountInSec;
            RemainingTimeInSec = Mathf.Max(0f, RemainingTimeInSec);
            
            OnRemainingTimeChanged?.Invoke();
        }
        
        private void PerformCountdownEndOperations()
        {
            PauseCountdown();

            //new OpenCountdownEndedScreenCommand().Execute();
            
            OnCountdownEnded?.Invoke();
        }
        
        private IEnumerator CountdownCoroutine()
        {
            while (true)
            {
                while (_isPaused)
                {
                    yield return new WaitForEndOfFrame();
                }
                
                yield return new WaitForSeconds(0.25f);

                RemoveDurationFromCountdown(0.25f);
                
                if (RemainingTimeInSec < float.Epsilon)
                {
                    PerformCountdownEndOperations();
                    
                    yield return new WaitWhile(() => RemainingTimeInSec < float.Epsilon);                
                }
            }
        }
        
        #endregion

        #region Info Methods
        
        public string GetRemainingTimeText()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(RemainingTimeInSec);
            return $"{(int)timeSpan.TotalMinutes}:{timeSpan.Seconds:D2}";
        }

        #endregion
    }
}