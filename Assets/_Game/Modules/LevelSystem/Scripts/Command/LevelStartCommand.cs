using Library.ServiceLocatorSystem;
using Modules.Gameplay;

namespace Modules.LevelSystem
{
    public class LevelStartCommand
    {
        private CountdownController CountdownController => ServiceLocator.Get<CountdownController>();
        
        public LevelStartCommand() { }

        public void Execute()
        {
            CountdownController.StartCountdown();
        }
    }
}