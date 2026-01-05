using UnityEngine;

namespace Library.ScreenManagement
{
    public class Utility
    {
        private const int InitialSortingOrder = 200;
        private const int SortingOrderIncreaseAmount = 20;
        
        public static int GetCalculatedSortingOrder(int sortingOrderIndex)
        {
            return InitialSortingOrder + SortingOrderIncreaseAmount * sortingOrderIndex;
        }
        
        public static bool IsPrefab(GameObject go)
        {
            return go.scene.name == null;
        }
    }
}