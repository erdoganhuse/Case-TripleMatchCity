using Cysharp.Threading.Tasks;
using Library.ServiceLocatorSystem;
using Modules.Gameplay;
using Modules.UI;

namespace Modules.LevelSystem
{
    public class LevelWinCommand
    {
        private LoadingPanel LoadingPanel => ServiceLocator.Get<LoadingPanel>();
        private CountdownController CountdownController => ServiceLocator.Get<CountdownController>();
        private InputController InputController => ServiceLocator.Get<InputController>();
        private UiController UiController => ServiceLocator.Get<UiController>();
        private LevelController LevelController => ServiceLocator.Get<LevelController>();

        public LevelWinCommand() { }

        public void Execute()
        {
            ExecuteAsync().Forget();
        }

        private async UniTask ExecuteAsync()
        {
            CountdownController.PauseCountdown();
            InputController.DisableInput();

            await UniTask.WaitForSeconds(1f);
            
            bool isContinueAtWin = false;
            UiController.OpenWinScreen(() => isContinueAtWin = true);
            
            await UniTask.WaitUntil(() => isContinueAtWin);
            
            LoadingPanel.Show();

            await UniTask.WaitForSeconds(0.3f);
            
            LevelController.ClearLevel();
            
            await UniTask.WaitForSeconds(0.3f);
            
            new OpenLevelFromGameplayCommand().Execute();
        }
    }
}