using Library.ScreenManagement;
using Library.ServiceLocatorSystem;
using Modules.Gameplay;
using Modules.LevelSystem;
using Modules.UI;
using ThirdParty.Other.EditorButton;
using UnityEngine;

namespace Modules.Shared
{
    public static class GameplayBindingIds
    {
        public const string LevelRoot = "LevelRoot";
    }
    
    public class GameplayInstaller : BaseInstaller
    {
        [SerializeField] private Transform _levelRoot;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private CountdownController _countdownController;
        [SerializeField] private InputController _inputController;
        [SerializeField] private ItemCollectController _itemCollectController;
        [SerializeField] private TargetController _targetController;
        [SerializeField] private NavigationController _navigationController;
        [SerializeField] private LevelController _levelController;
        [SerializeField] private LevelStatusCheckController _levelStatusCheckController;
        
        [Header("UI")]
        [SerializeField] private UiController _uiController;
        [SerializeField] private ScreenManager _screenManager;
        
        protected override void InstallBindings()
        {
            ServiceLocator.BindInstance(_levelRoot, GameplayBindingIds.LevelRoot);
            ServiceLocator.BindInstance(_cameraController);
            ServiceLocator.BindInstance(_countdownController);
            ServiceLocator.BindInstance(_inputController);
            ServiceLocator.BindInstance(_itemCollectController);
            ServiceLocator.BindInstance(_targetController);
            ServiceLocator.BindInstance(_navigationController);
            ServiceLocator.BindInstance(_levelController);
            ServiceLocator.BindInstance(_levelStatusCheckController);
            
            ServiceLocator.BindInstance(_uiController);
            ServiceLocator.BindInstance(_screenManager);
        }

        [EditorButton]
        public void DeletePlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
