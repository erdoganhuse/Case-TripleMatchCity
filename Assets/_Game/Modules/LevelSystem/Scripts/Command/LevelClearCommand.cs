using Library.ServiceLocatorSystem;
using Modules.Gameplay;
using Modules.UI;
using UnityEngine;

namespace Modules.LevelSystem
{
    public class LevelClearCommand
    {
        private CountdownController CountdownController => ServiceLocator.Get<CountdownController>();
        private ItemCollectController ItemCollectController => ServiceLocator.Get<ItemCollectController>();
        private TargetController TargetController => ServiceLocator.Get<TargetController>();
        private NavigationController NavigationController => ServiceLocator.Get<NavigationController>();
        private LevelStatusCheckController LevelStatusCheckController => ServiceLocator.Get<LevelStatusCheckController>();
        private UiController UiController => ServiceLocator.Get<UiController>();

        public LevelClearCommand() { }

        public void Execute()
        {            
            CountdownController.Clear();
            ItemCollectController.Clear();
            TargetController.Clear();
            NavigationController.Clear();
            LevelStatusCheckController.Clear();
            UiController.CloseAll();
            
            Resources.UnloadUnusedAssets();
        }
    }
}