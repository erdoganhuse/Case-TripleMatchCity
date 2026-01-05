using Cysharp.Threading.Tasks;
using Library.ServiceLocatorSystem;
using Modules.LevelSystem;
using Modules.UI;

namespace Modules.Gameplay
{
    public class OpenLevelFromGameplayCommand
    {
        private LoadingPanel LoadingPanel => ServiceLocator.Get<LoadingPanel>();
        private InputController InputController => ServiceLocator.Get<InputController>();
        private UiController UiController => ServiceLocator.Get<UiController>();
        private LevelController LevelController => ServiceLocator.Get<LevelController>();
        
        public OpenLevelFromGameplayCommand() { }

        public void Execute()
        {
            ExecuteAsync().Forget();
        }

        private async UniTask ExecuteAsync()
        {
            InputController.DisableInput();

            LoadingPanel.Show();
            
            LevelController.PrepareLevel();
            
            await UniTask.WaitForSeconds(0.3f);
            
            UiController.OpenInGameScreen();
            
            bool isPlayAtPreGameScreen = false;
            UiController.OpenPreGameScreen(() => isPlayAtPreGameScreen = true);
            
            await UniTask.WaitForSeconds(0.3f);

            LoadingPanel.Hide();
            
            await UniTask.WaitUntil(() => isPlayAtPreGameScreen);

            await UniTask.WaitForSeconds(0.3f);
            
            LevelController.StartLevel();
            
            InputController.EnableInput();
        }
    }
}