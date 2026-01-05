using Library.ServiceLocatorSystem;
using Modules.UI;
using Modules.UserData;
using UnityEngine;

namespace Modules.Shared
{
    public static class MainBindingIds
    {
        public const string MainSceneCanvas = "MainSceneCanvas";
    }
    
    public class MainInstaller : BaseInstaller
    {
        [SerializeField] private Canvas _mainSceneCanvas;
        [SerializeField] private LoadingPanel _loadingPanel;

        protected override void InstallBindings()
        {
            ServiceLocator.BindInstance(_mainSceneCanvas, MainBindingIds.MainSceneCanvas);
            ServiceLocator.BindInstance(_loadingPanel);
            
            ServiceLocator.Bind(new UserDataController());
        }
    }
}