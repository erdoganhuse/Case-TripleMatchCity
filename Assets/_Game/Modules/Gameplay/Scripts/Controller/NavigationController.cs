using Library.ServiceLocatorSystem;
using Modules.Shared;
using UnityEngine;

namespace Modules.Gameplay
{
    public class NavigationController : MonoBehaviour
    {
        private Transform LevelRoot => ServiceLocator.Get<Transform>(GameplayBindingIds.LevelRoot);
        private CameraController CameraController => ServiceLocator.Get<CameraController>();
        private InputController InputController => ServiceLocator.Get<InputController>();

        private float _maxZoomCameraOrthoSize = 2.5f;
        private float _minZoomCameraOrthoSize = 10f;
        private bool _shouldMoveLevelRoot;

        public void Setup()
        {
            CameraController.SetOrthographicSize(_minZoomCameraOrthoSize);
            ResetMovementPosition();
            
            RegisterEvents();
        }

        public void Clear()
        {
            UnregisterEvents();
        }
        
        public void Update()
        {
            if (_shouldMoveLevelRoot)
            {
                UpdateMovementPosition();
            }
        }

        private void UpdateMovementPosition()
        {
            LevelRoot.position += new Vector3(
                InputController.TouchPositionDelta.x, 
                InputController.TouchPositionDelta.y,
                0f);
            
            LimitMovementPosition();
        }
        
        private void LimitMovementPosition()
        {
            float posX = LevelRoot.position.x;
            float posY = LevelRoot.position.y;
            posX = Mathf.Clamp(posX, -2f, 2);
            posY = Mathf.Clamp(posY, -3f, 3);
            LevelRoot.position = new Vector3(posX, posY, 0f);
        }

        private void ResetMovementPosition()
        {
            LevelRoot.position = new Vector3(0f, 0f, 0f);
        }
        
        #region Event Methods

        private void RegisterEvents()
        {
            InputController.OnTouchStart += OnTouchStart;
            InputController.OnTouchEnd += OnTouchEnd;
        }

        private void UnregisterEvents()
        {
            InputController.OnTouchStart -= OnTouchStart;
            InputController.OnTouchEnd -= OnTouchEnd;
        }
        
        private void OnTouchStart()
        {
            _shouldMoveLevelRoot = true;
        }
        
        private void OnTouchEnd()
        {
            _shouldMoveLevelRoot = false;
        }

        #endregion
    }
}