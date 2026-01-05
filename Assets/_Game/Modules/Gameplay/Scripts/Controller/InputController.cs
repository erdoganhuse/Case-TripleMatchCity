using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Modules.Gameplay
{
    public class InputController : MonoBehaviour
    {
        public static bool IsInputEnabled { get; private set; } = false;

        public event Action OnTouchStart;
        public event Action OnTouchEnd;
        
        private const float MaxRaycastDistance = 1000f;
        private const float ClickActivationTimeThreshold = 0.3f;
        private const float ClickActivationDistanceThreshold = 0.2f;
        private const float CircleCastRadius = 0.5f;
        
        public Vector3 TouchPositionDelta { get; private set; }

        [SerializeField] private string _clickLayerName;
        
        private Camera _mainCamera;
        private Ray _ray;
        private RaycastHit2D[] _hits = new RaycastHit2D[20];

        private Vector3 _previousWorldTouchPos;
        private Vector3 _currentWorldTouchPos;
        private bool _isTouchStarted;
        private bool _isTouchStartedOverUi;
        private bool _isPointerOverUi;
        private float _touchStartTime;
        private Vector3 _touchStartWorldPos;
        private BaseItem _clickedItemAtTouchStart;

        public void EnableInput()
        {
            IsInputEnabled = true;
        }

        public void DisableInput()
        {
            IsInputEnabled = false;
        }
        
        private void Start()
        {
            _mainCamera = Camera.main;
        }
        
        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _currentWorldTouchPos = GetTouchWorldPosition();
                _previousWorldTouchPos = _currentWorldTouchPos;
            }
            
            _currentWorldTouchPos = GetTouchWorldPosition();
            TouchPositionDelta = _currentWorldTouchPos - _previousWorldTouchPos;
            _previousWorldTouchPos = _currentWorldTouchPos;
            _isPointerOverUi = IsPointerOverUi();
            
            if (Input.GetMouseButtonDown(0))
            {
                _isTouchStartedOverUi = _isPointerOverUi && Input.GetMouseButtonDown(0);
                _touchStartTime = Time.unscaledTime;
                _touchStartWorldPos = GetTouchWorldPosition();
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isTouchStartedOverUi = false;
            }

            if (_isTouchStarted && !IsInputEnabled)
            {
                _isTouchStarted = false;
                OnTouchEnd?.Invoke();
            }
            
            if (!IsInputEnabled || IsPointerOverUi()) return;
            
            if (!_isTouchStarted && Input.GetMouseButton(0) && !_isTouchStartedOverUi)
            {
                if (GetClickedItem() == null)
                {
                    _isTouchStarted = true;
                    OnTouchStart?.Invoke();
                }

                if (Time.unscaledTime - _touchStartTime > ClickActivationTimeThreshold)
                {
                    _isTouchStarted = true;
                    OnTouchStart?.Invoke();
                }
            }
            
            if (!Input.GetMouseButton(0) && _isTouchStarted)
            {
                _isTouchStarted = false;
                OnTouchEnd?.Invoke();
            }
            
            ControlItemClick();
        }

        private bool IsPointerOverUi()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
        
        private Vector3 GetTouchWorldPosition()
        {
            _ray = GetMouseScreenPointRay();
            
            if (Physics2D.RaycastNonAlloc(_ray.origin, _ray.direction, _hits, MaxRaycastDistance, 1 << LayerMask.NameToLayer(_clickLayerName)) > 0)
            {
                Vector3 newTouchWorldPosition = _hits[0].point;
                return newTouchWorldPosition;
            }

            return Vector3.zero;
        }
        
        private Ray GetMouseScreenPointRay()
        {
            return _mainCamera.ScreenPointToRay(Input.mousePosition);
        }
        
        private BaseItem GetClickedItem()
        {
            BaseItem closestItem = null;
            float minDistance = float.MaxValue;
            
            _ray = GetMouseScreenPointRay();
            RaycastHit2D[] hits = Physics2D.CircleCastAll(_ray.origin, CircleCastRadius, _ray.direction, MaxRaycastDistance);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                float distance = Vector3.Distance(hit.point, _ray.origin);
                BaseItem item = hit.collider.gameObject.GetComponentInParent<BaseItem>();
                if (item != null && item.IsInteractable() && distance < minDistance)
                {
                    closestItem = item;
                    minDistance = distance;
                }
            }
            
            return closestItem;
        }

        private void ControlItemClick()
        {
            bool isClickingToItem = GetClickedItem() != null;
            if (Input.GetMouseButtonDown(0) && isClickingToItem)
            {
                _clickedItemAtTouchStart = GetClickedItem();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                BaseItem clickedItem = GetClickedItem();
                if (clickedItem != null && _clickedItemAtTouchStart == clickedItem)
                {
                    bool isMoveMarginAcceptable = (_touchStartWorldPos - GetTouchWorldPosition()).magnitude <
                                                  ClickActivationDistanceThreshold;
                    bool isTimeMarginAcceptable = Time.unscaledTime - _touchStartTime <= ClickActivationTimeThreshold;

                    if (isMoveMarginAcceptable && isTimeMarginAcceptable)
                    {
                        new ItemCollectCommand(clickedItem).Execute();
                    }
                }
                _clickedItemAtTouchStart = null;
            }
        }
    }
}