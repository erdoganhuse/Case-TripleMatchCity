using Cysharp.Threading.Tasks;
using DG.Tweening;
using Modules.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Modules.Shared
{
    public class GameInitializer : MonoBehaviour
    { 
        private void Start()
        {
            InitializeAsync().Forget();
        }

        private async UniTask InitializeAsync()
        {
            Application.targetFrameRate = 61;
            Input.multiTouchEnabled = false;
            DOTween.defaultTimeScaleIndependent = true;
            
            await UniTask.WaitForSeconds(0.5f);

            await SceneManager.LoadSceneAsync(SceneNames.Gameplay, LoadSceneMode.Additive);
            
            await UniTask.WaitForSeconds(0.2f);

            Scene gameplayScene = SceneManager.GetSceneByName(SceneNames.Gameplay);
            SceneManager.SetActiveScene(gameplayScene);

            await UniTask.WaitForSeconds(0.2f);
            
            new OpenLevelFromGameplayCommand().Execute();
        }
    }
}