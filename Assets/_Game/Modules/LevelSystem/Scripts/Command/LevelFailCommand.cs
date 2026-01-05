using Cysharp.Threading.Tasks;
using Library.ServiceLocatorSystem;
using Modules.Gameplay;
using Modules.UI;

namespace Modules.LevelSystem
{
    public class LevelFailCommand
    {
        private LoadingPanel LoadingPanel => ServiceLocator.Get<LoadingPanel>();
        private InputController InputController => ServiceLocator.Get<InputController>();
        private CountdownController CountdownController => ServiceLocator.Get<CountdownController>();
        private LevelController LevelController => ServiceLocator.Get<LevelController>();
        private UiController UiController => ServiceLocator.Get<UiController>();
        
        public LevelFailCommand() { }

        public void Execute()
        {
            ExecuteAsync().Forget();
        }

        private async UniTask ExecuteAsync()
        {
            CountdownController.PauseCountdown();
            InputController.DisableInput();
            
            await UniTask.WaitForSeconds(0.3f);
            
            bool isTryAgain = false;
            UiController.OpenFailScreen(() => isTryAgain = true);
            await UniTask.WaitUntil(() => isTryAgain);
            
            LoadingPanel.Show();

            await UniTask.WaitForSeconds(0.3f);
            
            LevelController.ClearLevel();
            
            await UniTask.WaitForSeconds(0.3f);
            
            new OpenLevelFromGameplayCommand().Execute();
        }
    }
}