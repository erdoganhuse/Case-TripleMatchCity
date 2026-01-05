using System;
using System.Globalization;

namespace Library.Utility
{
    public enum TimeSpanTextFormatType
    {
        MinSec,
        HourMinSec,
        HourMin,
        DayHourMin,
        DayHour
    }
    
    public static class BasicExtensionMethods
    {
        public static string ToSpacedAmountText(this int amount)
        {
            if (amount == 0)
            {
                return "0";
            }

            return amount.ToString("### ###", CultureInfo.InvariantCulture);
        }
        
        public static string ToRemainingTime(this float seconds)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
            return $"{(int)timeSpan.TotalMinutes}:{timeSpan.Seconds:D2}";
        }
        
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
}