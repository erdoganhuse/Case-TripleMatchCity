using System;
using System.Collections;
using System.Collections.Generic;
using Library.CoroutineSystem;
using TMPro;
using UnityEngine;

namespace Library.PersistentData
{
    public enum TimeSpanTextFormatType
    {
        MinSec,
        HourMinSec,
        HourMin,
        DayHourMin,
        DayHour
    }
    
    public static class TimeSpanExtensionMethods
    {
        public static string ToFormattedText(this TimeSpan timeSpan, TimeSpanTextFormatType textFormatType, string completedString = "Completed")
        {
            if (timeSpan.Seconds <= 0f)
            {
                return completedString;
            }
            
            switch (textFormatType)
            {
                case TimeSpanTextFormatType.MinSec:
                    return $"{timeSpan.Minutes.ToString()}:{timeSpan.Minutes.ToString()}";
                case TimeSpanTextFormatType.HourMinSec:
                    return $"{timeSpan.Hours.ToString()}:{timeSpan.Minutes.ToString()}:{timeSpan.Minutes.ToString()}";
                case TimeSpanTextFormatType.HourMin:
                    return $"{timeSpan.Hours.ToString()}h {timeSpan.Minutes.ToString()}m";
                case TimeSpanTextFormatType.DayHourMin:
                    return $"{timeSpan.Days.ToString()}d {timeSpan.Hours.ToString()}h {timeSpan.Minutes.ToString()}m";
                case TimeSpanTextFormatType.DayHour:
                    return $"{timeSpan.Days.ToString()}d {timeSpan.Hours.ToString()}h";
                default:
                    return default;
            }
        }
    }
    
    public class PersistentCountdownTimer
    {
        private readonly TimeSpan _duration;
        private readonly TimeSpanTextFormatType _textFormatType;
        
        private readonly PersistentVariable<DateTime> _countdownStartTime;
        private readonly List<TextMeshProUGUI> _bindTexts;
        private readonly List<Action> _onCompletedListeners = new();

        private Coroutine _timerCoroutine;

        public PersistentCountdownTimer(string key, TimeSpan duration, TimeSpanTextFormatType textFormatType = TimeSpanTextFormatType.MinSec)
        {
            _duration = duration;
            _textFormatType = textFormatType;
            _countdownStartTime = new(key, new DateTime(2023, 1, 1));
            _bindTexts = new List<TextMeshProUGUI>();
        }

        public bool IsCompleted()
        {
            return GetPassedDuration() > _duration;
        }

        public void SetStartTime(DateTime dateTime)
        {
            _countdownStartTime.Value = dateTime;
            _countdownStartTime.ForceSave();
        }

        public DateTime GetStartTime()
        {
            return _countdownStartTime.Value;
        }
        
        public TimeSpan GetRemainingDuration()
        {
            return _countdownStartTime.Value.Add(_duration).Subtract(DateTime.UtcNow);
        }
        
        public TimeSpan GetPassedDuration()
        {
            return DateTime.UtcNow.Subtract(_countdownStartTime.Value);
        }

        public void AddListenerToCompleted(Action onCompleted)
        {
            if (!_onCompletedListeners.Contains(onCompleted))
            {
                _onCompletedListeners.Add(onCompleted);
            }
        }

        public void RemoveListenerToCompleted(Action onCompleted)
        {
            if (_onCompletedListeners.Contains(onCompleted))
            {
                _onCompletedListeners.Remove(onCompleted);
            }
        }
        
        #region Timer Methods

        public void StartTimer()
        {
            if (_timerCoroutine != null)
            {
                StopTimer();
            }

            _timerCoroutine = CoroutineManager.BeginCoroutine(TimerCoroutine());
        }

        public void StopTimer()
        {
            CoroutineManager.EndCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }

        private IEnumerator TimerCoroutine()
        {
            while (!IsCompleted())
            {
                string timerText = GetRemainingDuration().ToFormattedText(_textFormatType);
                for (int i = 0; i < _bindTexts.Count; i++)
                {
                    _bindTexts[i].text = timerText;
                }

                yield return new WaitForSeconds(1f);
            }

            StopTimer();

            for (int i = 0; i < _onCompletedListeners.Count; i++)
            {
                _onCompletedListeners[i]?.Invoke();
            }
        }

        #endregion
        
        #region UI Methods
        
        public void BindText(TextMeshProUGUI text)
        {
            if (!_bindTexts.Contains(text))
            {
                text.text = GetRemainingDuration().ToFormattedText(_textFormatType);
                _bindTexts.Add(text);
            }
        }

        public void UnbindText(TextMeshProUGUI text)
        {
            if (_bindTexts.Contains(text))
            {
                _bindTexts.Remove(text);
            }
        }
        
        protected void ClearBoundTexts()
        {
            _bindTexts.Clear();
        }

        #endregion
    }
}