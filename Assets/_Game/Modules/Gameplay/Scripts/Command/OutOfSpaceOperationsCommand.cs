using Cysharp.Threading.Tasks;
using Library.ServiceLocatorSystem;
using Modules.LevelSystem;
using Modules.UI;

namespace Modules.Gameplay
{
    public class OutOfSpaceOperationsCommand
    {
        private InputController InputController => ServiceLocator.Get<InputController>();
        private CountdownController CountdownController => ServiceLocator.Get<CountdownController>();
        private ItemCollectController ItemCollectController => ServiceLocator.Get<ItemCollectController>();
        private LevelController LevelController => ServiceLocator.Get<LevelController>();
        private UiController UiController => ServiceLocator.Get<UiController>();
        
        public OutOfSpaceOperationsCommand() { }

        public void Execute()
        {
            ExecuteAsync().Forget();
        }

        private async UniTask ExecuteAsync()
        {
            InputController.DisableInput();
            CountdownController.PauseCountdown();
            
            await UniTask.WaitForSeconds(1f);

            bool isContinue = false;
            bool isGiveUp = false;
            UiController.OpenOutOfSpaceScreen(() => isContinue = true, () => isGiveUp = true);
            await UniTask.WaitUntil(() => isContinue || isGiveUp);

            if (isContinue)
            {
                ItemCollectController.ClearAllSlots();

                await UniTask.WaitForSeconds(0.5f);
                
                InputController.EnableInput();
                CountdownController.ResumeCountdown();
            }
            else
            {
                LevelController.FailLevel();
            }
        }
    }
}