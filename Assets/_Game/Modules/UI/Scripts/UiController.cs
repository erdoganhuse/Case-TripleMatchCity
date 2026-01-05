using System;
using Library.ScreenManagement;
using Library.ServiceLocatorSystem;
using Modules.Gameplay;
using Modules.UserData;
using UnityEngine;

namespace Modules.UI
{
    public class UiController : MonoBehaviour
    {
        private CountdownController CountdownController => ServiceLocator.Get<CountdownController>();
        private ScreenManager ScreenManager => ServiceLocator.Get<ScreenManager>();
        private UserDataController UserDataController => ServiceLocator.Get<UserDataController>();

        public void CloseAll()
        {
            ScreenManager.CloseAll();
        }
        
        public void OpenPreGameScreen(Action onPlay = null)
        {
            PreGameScreenArgs args = new();
            args.LevelNo = UserDataController.GetLevel();
            args.OnBoosterSelected = (boosterId) =>
            {
                Debug.Log($"Booster Selected: {boosterId}");
            };
            args.OnPlay = () =>
            {
                onPlay?.Invoke();
                ScreenManager.Close<PreGameScreen>();
            };
            
            ScreenManager.Open<PreGameScreen>(args);
        }

        public void OpenInGameScreen()
        {
            InGameScreenArgs args = new();
            args.LevelNo = UserDataController.GetLevel();
            ScreenManager.Open<InGameScreen>(args);
        }

        public void OpenWinScreen(Action onContinue = null)
        {
            WinScreenArgs args = new();
            args.GainedStarCount = 3;
            args.RemainingDurationText = CountdownController.GetRemainingTimeText();
            args.OnContinue = () =>
            {
                onContinue?.Invoke();
                ScreenManager.Close<WinScreen>();
            };
            ScreenManager.Open<WinScreen>(args);
        }

        public void OpenOutOfTimeScreen(Action onContinue = null, Action onGiveUp = null)
        {
            OutOfTimeScreenArgs args = new();
            args.AdditionalTimeText = (60).ToString();
            args.OnGiveUp = () =>
            {
                ScreenManager.Close<OutOfTimeScreen>();
                onGiveUp?.Invoke();
            };
            args.OnAddTime = () =>
            {
                //ToDo: Control Coin Amount
                ScreenManager.Close<OutOfTimeScreen>();
                onContinue?.Invoke();
            };
            ScreenManager.Open<OutOfTimeScreen>(args);
        }

        public void OpenOutOfSpaceScreen(Action onContinue = null, Action onGiveUp = null)
        {
            OutOfSpaceScreenArgs args = new();
            args.OnGiveUp = () =>
            {
                ScreenManager.Close<OutOfSpaceScreen>();
                onGiveUp?.Invoke();
            };
            args.OnClearSpace = () =>
            {
                //ToDo: Control Coin Amount
                ScreenManager.Close<OutOfSpaceScreen>();
                onContinue?.Invoke();
            };
            ScreenManager.Open<OutOfSpaceScreen>(args);
        }

        public void OpenFailScreen(Action onTryAgain = null)
        {
            FailScreenArgs args = new();
            args.OnBoosterSelected = (boosterId) =>
            {
                Debug.Log($"Booster Selected: {boosterId}");
            };
            args.OnTryAgain = () =>
            {
                onTryAgain?.Invoke();
                ScreenManager.Close<FailScreen>();
            };
            
            ScreenManager.Open<FailScreen>(args);
        }
    }
}