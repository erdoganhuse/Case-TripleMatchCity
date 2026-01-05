using Library.ServiceLocatorSystem;
using Modules.Gameplay;
using Modules.UserData;
using UnityEngine;

namespace Modules.LevelSystem
{
    public class LevelController : MonoBehaviour
    {
        private UserDataController UserDataController => ServiceLocator.Get<UserDataController>();

        [SerializeField] private Transform _levelRoot;
        [SerializeField] private LevelCollection _levelCollection;

        private Level _currentLevel;
        
        public void PrepareLevel()
        {
            int levelIndex = UserDataController.GetLevel() - 1;
            LevelData levelData = _levelCollection.GetLevelData(levelIndex);
            Level levelPrefab = Resources.Load<Level>(levelData.PrefabDir);
            
            _currentLevel = Instantiate(levelPrefab, _levelRoot);
            _currentLevel.transform.localPosition = Vector3.zero;
            
            new LevelPrepareCommand(_currentLevel).Execute();
        }
        
        public void ClearLevel()
        {
            if (_currentLevel != null)
            {
                DestroyImmediate(_currentLevel.gameObject);
                _currentLevel = null;   
            }
            
            new LevelClearCommand().Execute();
        }
        
        public void StartLevel()
        {
            new LevelStartCommand().Execute();
        }

        public void WinLevel()
        {
            UserDataController.IncrementLevel();
            new LevelWinCommand().Execute();
        }

        public void FailLevel()
        {
            new LevelFailCommand().Execute();
        }
    }
}