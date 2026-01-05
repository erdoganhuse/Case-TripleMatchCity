using Library.ServiceLocatorSystem;
using Modules.Gameplay;
using TMPro;
using UnityEngine;

namespace Modules.UI
{
    public class CountdownInfoPanel : MonoBehaviour
    {
        private CountdownController CountdownController => ServiceLocator.Get<CountdownController>();

        [SerializeField] private TextMeshProUGUI _remainingTimeText;
        
        private void Start()
        {
            CountdownController.OnRemainingTimeChanged += OnRemainingTimeChanged;
        }

        private void OnDestroy()
        {
            CountdownController.OnRemainingTimeChanged -= OnRemainingTimeChanged;
        }

        public void Setup()
        {
            UpdateRemainingTimeText();   
        }

        public void Clear() { }
        
        private void UpdateRemainingTimeText()
        {
            _remainingTimeText.text = CountdownController.GetRemainingTimeText();
        }
        
        private void OnRemainingTimeChanged()
        {
            UpdateRemainingTimeText();
        }
    }
}