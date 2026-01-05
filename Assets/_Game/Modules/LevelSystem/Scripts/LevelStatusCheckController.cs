using Library.ServiceLocatorSystem;
using Modules.Gameplay;
using UnityEngine;

namespace Modules.LevelSystem
{
    public class LevelStatusCheckController : MonoBehaviour
    {
        private CountdownController CountdownController => ServiceLocator.Get<CountdownController>();
        private ItemCollectController ItemCollectController => ServiceLocator.Get<ItemCollectController>();
        private TargetController TargetController => ServiceLocator.Get<TargetController>();
        private LevelController LevelController => ServiceLocator.Get<LevelController>();
        
        public void Setup()
        {
            RegisterEvents();
        }

        public void Clear()
        {
            UnregisterEvents();
        }

        #region Event Methods

        private void RegisterEvents()
        {
            CountdownController.OnCountdownEnded += OnCountdownEnded;
            ItemCollectController.OnAllSlotsFilled += OnAllSlotsFilled;
            TargetController.OnAllTargetsCompleted += OnAllTargetsCompleted;
        }

        private void UnregisterEvents()
        {
            CountdownController.OnCountdownEnded -= OnCountdownEnded;
            ItemCollectController.OnAllSlotsFilled -= OnAllSlotsFilled;
            TargetController.OnAllTargetsCompleted -= OnAllTargetsCompleted;
        }
        
        private void OnAllTargetsCompleted()
        {
            LevelController.WinLevel();
        }

        private void OnAllSlotsFilled()
        {
            new OutOfSpaceOperationsCommand().Execute();
        }

        private void OnCountdownEnded()
        {
            new OutOfTimeOperationsCommand().Execute();
        }

        #endregion
    }
}