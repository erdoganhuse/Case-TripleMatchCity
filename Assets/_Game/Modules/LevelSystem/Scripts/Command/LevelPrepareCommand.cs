using Library.ServiceLocatorSystem;
using Modules.Gameplay;

namespace Modules.LevelSystem
{
    public class LevelPrepareCommand
    {
        private CountdownController CountdownController => ServiceLocator.Get<CountdownController>();
        private ItemCollectController ItemCollectController => ServiceLocator.Get<ItemCollectController>();
        private TargetController TargetController => ServiceLocator.Get<TargetController>();
        private NavigationController NavigationController => ServiceLocator.Get<NavigationController>();
        private LevelStatusCheckController LevelStatusCheckController => ServiceLocator.Get<LevelStatusCheckController>();

        private readonly Level _level;
        
        public LevelPrepareCommand(Level level)
        {
            _level = level;
        }

        public void Execute()
        {
            CountdownController.Setup(_level.GetDuration());
            ItemCollectController.Setup();
            TargetController.Setup(_level.GetTargetDatas());
            NavigationController.Setup();
            LevelStatusCheckController.Setup();
        }
    }
}