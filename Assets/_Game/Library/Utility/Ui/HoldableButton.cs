using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Library.Utility
{
    public class HoldableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private UnityEvent _onHoldStart;
        [SerializeField] private UnityEvent _onHoldEnd;

        public void OnPointerUp(PointerEventData eventData)
        {
            _onHoldEnd?.Invoke();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _onHoldStart?.Invoke();
        }
    }
}