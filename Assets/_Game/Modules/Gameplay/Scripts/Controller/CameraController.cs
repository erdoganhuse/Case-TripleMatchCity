using UnityEngine;

namespace Modules.Gameplay
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        
        public void SetOrthographicSize(float value)
        {
            _mainCamera.orthographicSize = value;
        }
    }
}