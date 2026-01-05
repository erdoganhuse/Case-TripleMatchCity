using UnityEngine;

namespace Modules.LevelSystem
{
    [CreateAssetMenu(fileName = "New LevelCollection", menuName = "Modules/LevelSystem/LevelCollection", order = 1)]
    public class LevelCollection : ScriptableObject
    {
        [SerializeField] private LevelData[] _levelDatas;
        
        public LevelData GetLevelData(int levelIndex)
        {
            int normalizedLevelIndex = levelIndex % _levelDatas.Length;
            return _levelDatas[normalizedLevelIndex];
        }
    }
}