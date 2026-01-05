using UnityEngine;

namespace Modules.UI
{
    public class LoadingPanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}